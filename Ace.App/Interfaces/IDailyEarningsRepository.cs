using System;
using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IDailyEarningsRepository
    {
        void AddDailyEarningsDetails(List<DailyEarningsDetail> details);
        List<DailyEarningsDetail> GetDetailsByDate(DateTime date);
        List<DailyEarningsDetail> GetDetailsByYearMonth(int year, int month);
        List<DailyEarningsDetail> GetDetailsByYear(int year);
        List<DailyEarningsDetail> GetAllDetails();
    }
}
