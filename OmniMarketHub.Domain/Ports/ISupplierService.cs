using System.Threading.Tasks;
using OmniMarketHub.Domain.Entities;

namespace OmniMarketHub.Domain.Ports
{
    // Domain'in dış dünyadan isteği: "Benim için şu üründen sipariş geç"
    public interface ISupplierService
    {
        Task OrderStockAsync(Product product, int quantity);
    }
}