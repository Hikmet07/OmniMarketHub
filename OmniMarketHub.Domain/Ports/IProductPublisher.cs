using System.Threading.Tasks;
using OmniMarketHub.Domain.Entities; // Burası Product.cs'nin adresini gösteriyor

namespace OmniMarketHub.Domain.Ports
{
    public interface IProductPublisher
    {
        // Product kelimesi artık kızarmamalı çünkü yukarıda "using ... Entities" dedik.
        Task PublishProductAsync(Product product);
    }
}