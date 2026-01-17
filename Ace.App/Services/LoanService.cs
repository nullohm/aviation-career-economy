using System;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoggingService _logger;
        private readonly IFinanceService _financeService;
        private readonly Func<AceDbContext> _dbContextFactory;
        private const decimal InterestRate = 0.10m;

        public event Action? LoansChanged;

        public LoanService(ILoggingService logger, IFinanceService financeService, Func<AceDbContext>? dbContextFactory = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _dbContextFactory = dbContextFactory ?? (() => new AceDbContext());
        }

        public decimal GetTotalOutstandingLoans()
        {
            try
            {
                using var db = _dbContextFactory();
                db.Database.EnsureCreated();
                var outstanding = db.Loans
                    .Where(l => !l.IsRepaid)
                    .Sum(l => (decimal?)l.TotalRepayment) ?? 0m;

                _logger.Debug($"LoanService: Total outstanding loans: {outstanding:C}");
                return outstanding;
            }
            catch (Exception ex)
            {
                _logger.Error("LoanService: Failed to calculate outstanding loans", ex);
                return 0m;
            }
        }

        public bool TakeLoan(decimal amount)
        {
            if (amount <= 0)
            {
                _logger.Warn($"LoanService: Invalid loan amount requested: {amount:C}");
                return false;
            }

            try
            {
                var totalRepayment = amount * (1 + InterestRate);

                var loan = new Loan
                {
                    Amount = amount,
                    InterestRate = InterestRate,
                    TotalRepayment = totalRepayment,
                    TakenDate = DateTime.Now,
                    IsRepaid = false
                };

                using var db = _dbContextFactory();
                db.Database.EnsureCreated();
                db.Loans.Add(loan);
                db.SaveChanges();

                _financeService.AddEarnings(amount, $"Loan taken (Repayment: {totalRepayment:N0} €)", null);

                _logger.Info($"LoanService: Loan taken - Amount: {amount:C}, Repayment: {totalRepayment:C}");

                LoansChanged?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"LoanService: Failed to process loan of {amount:C}", ex);
                return false;
            }
        }

        public bool RepayLoan(int loanId)
        {
            try
            {
                using var db = _dbContextFactory();
                db.Database.EnsureCreated();
                var loan = db.Loans.FirstOrDefault(l => l.Id == loanId && !l.IsRepaid);

                if (loan == null)
                {
                    _logger.Warn($"LoanService: Loan {loanId} not found or already repaid");
                    return false;
                }

                var currentBalance = _financeService.GetCurrentBalance();
                if (currentBalance < loan.TotalRepayment)
                {
                    _logger.Warn($"LoanService: Insufficient balance to repay loan {loanId}. Required: {loan.TotalRepayment:C}, Available: {currentBalance:C}");
                    return false;
                }

                loan.IsRepaid = true;
                loan.RepaidDate = DateTime.Now;
                db.SaveChanges();

                _financeService.AddExpense(loan.TotalRepayment, $"Loan repayment (Principal: {loan.Amount:N0} € + Interest: {(loan.TotalRepayment - loan.Amount):N0} €)");

                _logger.Info($"LoanService: Loan {loanId} repaid - Total: {loan.TotalRepayment:C}");

                LoansChanged?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"LoanService: Failed to repay loan {loanId}", ex);
                return false;
            }
        }
    }
}
