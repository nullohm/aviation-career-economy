using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface ITransactionRepository
    {
        List<Transaction> GetRecentTransactions(int count);
        List<Transaction> GetAllTransactions();
        Transaction? GetTransactionByFlightId(int flightId);
        void AddTransaction(Transaction transaction);
    }
}
