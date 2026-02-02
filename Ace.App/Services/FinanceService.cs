using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class FinanceService : IFinanceService, INotifyPropertyChanged
    {
        private readonly ILoggingService _logger;
        private decimal _balance;
        private decimal _totalEarnings;
        private List<Transaction> _recentTransactions = new();

        public decimal Balance
        {
            get => _balance;
            private set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BalanceFormatted));
                }
            }
        }

        public decimal TotalEarnings
        {
            get => _totalEarnings;
            private set
            {
                if (_totalEarnings != value)
                {
                    _totalEarnings = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Transaction> RecentTransactions
        {
            get => _recentTransactions;
            private set
            {
                _recentTransactions = value;
                OnPropertyChanged();
            }
        }

        public string BalanceFormatted => $"€ {Balance:N2}".Replace(",", ".");

        public FinanceService(ILoggingService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _balance = 0m;
            _totalEarnings = 0m;
            LoadTransactions();
        }

        public void AddEarnings(decimal amount, string description = "Income", int? flightId = null)
        {
            if (amount > 0)
            {
                Balance += amount;
                TotalEarnings += amount;

                var transaction = new Transaction
                {
                    Date = DateTime.Now,
                    Description = description,
                    Type = "Income",
                    Amount = amount,
                    FlightId = flightId
                };

                SaveTransaction(transaction);
                _logger.Info($"Earnings added: €{amount:N2}, New balance: €{Balance:N2}");
            }
        }

        public void AddExpense(decimal amount, string description = "Expense")
        {
            if (amount > 0)
            {
                Balance -= amount;

                var transaction = new Transaction
                {
                    Date = DateTime.Now,
                    Description = description,
                    Type = "Expense",
                    Amount = -amount
                };

                SaveTransaction(transaction);
                _logger.Info($"Expense deducted: €{amount:N2}, New balance: €{Balance:N2}");
            }
        }

        public void SetBalance(decimal amount)
        {
            Balance = amount;
        }

        public decimal GetCurrentBalance()
        {
            return Balance;
        }

        private void SaveTransaction(Transaction transaction)
        {
            try
            {
                using var db = new AceDbContext();
                db.Database.EnsureCreated();
                db.Transactions.Add(transaction);
                db.SaveChanges();
                LoadTransactions();
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to save transaction", ex);
            }
        }

        public void LoadTransactions()
        {
            try
            {
                using var db = new AceDbContext();
                RecentTransactions = db.Transactions
                    .Where(t => !t.Description.StartsWith("Daily earnings:") &&
                                !t.Description.StartsWith("Monthly summary:") &&
                                !t.Description.StartsWith("Yearly summary:"))
                    .OrderByDescending(t => t.Id)
                    .Take(20)
                    .ToList();
                Balance = db.Transactions.Sum(t => (decimal?)t.Amount) ?? 0m;

                _logger.Info($"Loaded {RecentTransactions.Count} transactions, Balance: €{Balance:N2}");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to load transactions", ex);
                RecentTransactions = new List<Transaction>();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
