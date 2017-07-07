using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface ITaxEstimationService
    {
        Task<double> EstimateTotalTax();
    }
}
