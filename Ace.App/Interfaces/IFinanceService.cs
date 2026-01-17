using System.Collections.Generic;
using System.ComponentModel;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IFinanceService : INotifyPropertyChanged
    {
        decimal Balance { get; }
        decimal TotalEarnings { get; }
        List<Transaction> RecentTransactions { get; }
        string BalanceFormatted { get; }

        void AddEarnings(decimal amount, string description = "Income", int? flightId = null);
        void AddExpense(decimal amount, string description = "Expense");
        void SetBalance(decimal amount);
        decimal GetCurrentBalance();
        void LoadTransactions();
    }
}
