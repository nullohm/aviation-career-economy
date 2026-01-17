using System.Threading.Tasks;

namespace Ace.App.Interfaces
{
    public interface IFBOBillingService
    {
        Task ProcessMonthlyFBOBilling();
    }
}
