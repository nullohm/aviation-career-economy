using System;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface ILoanService
    {
        event Action? LoansChanged;
        bool TakeLoan(decimal amount);
        bool RepayLoan(int loanId);
        decimal GetTotalOutstandingLoans();
    }
}
