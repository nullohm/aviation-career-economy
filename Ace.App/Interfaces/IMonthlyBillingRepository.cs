using System;
using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IMonthlyBillingRepository
    {
        void AddBillingDetails(List<MonthlyBillingDetail> details);
        List<MonthlyBillingDetail> GetDetailsByMonth(int year, int month);
        List<MonthlyBillingDetail> GetAllDetails();
    }
}
