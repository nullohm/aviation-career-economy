using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class LoanRepository : BaseRepository, ILoanRepository
    {
        public LoanRepository(ILoggingService logger) : base(logger) { }

        public List<Loan> GetActiveLoans()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Loans.Where(l => !l.IsRepaid).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"LoanRepository: Failed to get active loans: {ex.Message}");
                return new List<Loan>();
            }
        }

        public Loan? GetLoanById(int id)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Loans.FirstOrDefault(l => l.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Error($"LoanRepository: Failed to get loan {id}: {ex.Message}");
                return null;
            }
        }

        public void AddLoan(Loan loan)
        {
            try
            {
                using var db = new AceDbContext();
                db.Loans.Add(loan);
                db.SaveChanges();
                _logger.Info($"LoanRepository: Added loan of {loan.Amount:C}");
            }
            catch (Exception ex)
            {
                _logger.Error($"LoanRepository: Failed to add loan: {ex.Message}");
                throw;
            }
        }

        public void UpdateLoan(Loan loan)
        {
            try
            {
                using var db = new AceDbContext();
                db.Loans.Update(loan);
                db.SaveChanges();
                _logger.Info($"LoanRepository: Updated loan {loan.Id}");
            }
            catch (Exception ex)
            {
                _logger.Error($"LoanRepository: Failed to update loan: {ex.Message}");
                throw;
            }
        }

        public decimal GetTotalOutstanding()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Loans.Where(l => !l.IsRepaid).Sum(l => l.TotalRepayment);
            }
            catch (Exception ex)
            {
                _logger.Error($"LoanRepository: Failed to get total outstanding: {ex.Message}");
                return 0;
            }
        }
    }
}
