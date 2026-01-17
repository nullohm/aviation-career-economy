using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        public TransactionRepository(ILoggingService logger) : base(logger) { }

        public List<Transaction> GetRecentTransactions(int count)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Transactions
                    .OrderByDescending(t => t.Date)
                    .Take(count)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"TransactionRepository: Failed to get recent transactions: {ex.Message}");
                return new List<Transaction>();
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Transactions
                    .OrderByDescending(t => t.Date)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"TransactionRepository: Failed to get all transactions: {ex.Message}");
                return new List<Transaction>();
            }
        }

        public Transaction? GetTransactionByFlightId(int flightId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Transactions.FirstOrDefault(t => t.FlightId == flightId);
            }
            catch (Exception ex)
            {
                _logger.Error($"TransactionRepository: Failed to get transaction for flight {flightId}: {ex.Message}");
                return null;
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            try
            {
                using var db = new AceDbContext();
                db.Transactions.Add(transaction);
                db.SaveChanges();
                _logger.Info($"TransactionRepository: Added transaction - {transaction.Description}");
            }
            catch (Exception ex)
            {
                _logger.Error($"TransactionRepository: Failed to add transaction: {ex.Message}");
                throw;
            }
        }
    }
}
