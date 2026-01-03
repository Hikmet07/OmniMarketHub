using System;
using System.IO;
using System.Threading.Tasks;
using OmniMarketHub.Domain.Entities;
using OmniMarketHub.Domain.Ports;

namespace OmniMarketHub.Infrastructure.Adapters
{
    public class AmazonFileAdapter : IProductPublisher
    {
        private readonly string _basePath;

        public AmazonFileAdapter()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _basePath = Path.Combine(desktopPath, "MarketData", "Amazon_Outbox");

            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task PublishProductAsync(Product product)
        {
            // Amazon bizden XML istiyor gibi hayal edelim ve formatı değiştirelim.
            string xmlContent = $@"
<AmazonProduct>
    <ID>{product.Id}</ID>
    <Title>{product.Name}</Title>
    <Price currency='USD'>{product.Price}</Price>
    <Stock>{product.StockQuantity}</Stock>
</AmazonProduct>";

            string fileName = $"Amazon_{product.Name}_{product.Id}.xml";
            string fullPath = Path.Combine(_basePath, fileName);

            await File.WriteAllTextAsync(fullPath, xmlContent);

            Console.WriteLine($"[AmazonAdapter] XML dosyası oluşturuldu: {fullPath}");
        }
    }
}