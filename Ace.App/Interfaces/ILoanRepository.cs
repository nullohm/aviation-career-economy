using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface ILoanRepository
    {
        List<Loan> GetActiveLoans();
        Loan? GetLoanById(int id);
        void AddLoan(Loan loan);
        void UpdateLoan(Loan loan);
        decimal GetTotalOutstanding();
    }
}
