using System;

namespace Ace.App.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal TotalRepayment { get; set; }
        public DateTime TakenDate { get; set; }
        public DateTime? RepaidDate { get; set; }
        public bool IsRepaid { get; set; }
    }
}
