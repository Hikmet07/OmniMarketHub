using System.Threading.Tasks;
using OmniMarketHub.Domain.Entities;
using OmniMarketHub.Domain.Ports;
using System.Collections.Generic;

namespace OmniMarketHub.Infrastructure.Adapters
{
    // Bu sınıfın görevi: Kendisine verilen TÜM adaptörleri tek tek çalıştırmaktır.
    public class OmniChannelBroadcaster : IProductPublisher
    {
        // Elimizdeki tüm yayıncıların listesi
        private readonly IEnumerable<IProductPublisher> _publishers;

        public OmniChannelBroadcaster(IEnumerable<IProductPublisher> publishers)
        {
            _publishers = publishers;
        }

        public async Task PublishProductAsync(Product product)
        {
            System.Console.WriteLine("--- [OMNI-CHANNEL] Dağıtım Başlıyor ---");

            foreach (var publisher in _publishers)
            {
                // Sonsuz döngüye girmemesi için kendisini çağırmıyoruz
                if (publisher.GetType() == typeof(OmniChannelBroadcaster)) continue;

                await publisher.PublishProductAsync(product);
            }

            System.Console.WriteLine("--- [OMNI-CHANNEL] Tüm kanallara dağıtıldı ---");
        }
    }
}