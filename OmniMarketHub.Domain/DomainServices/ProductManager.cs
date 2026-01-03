using System.Threading.Tasks;
using OmniMarketHub.Domain.Entities;
using OmniMarketHub.Domain.Ports;

namespace OmniMarketHub.Domain.DomainServices
{
    public class ProductManager
    {
        private readonly IProductPublisher _publisher;
        private readonly ISupplierService _supplierService; // Yeni yetenek eklendi

        // Artık yönetici hem Yayıncıyı hem Tedarikçiyi tanıyor
        public ProductManager(IProductPublisher publisher, ISupplierService supplierService)
        {
            _publisher = publisher;
            _supplierService = supplierService;
        }

        public async Task ProcessProductAsync(Product product)
        {
            // --- KURAL 1: UYARI (10'un altı) ---
            if (product.StockQuantity < 10 && product.StockQuantity >= 5)
            {
                System.Console.WriteLine($"[UYARI] '{product.Name}' stoğu azalıyor! ({product.StockQuantity} adet kaldı)");
            }

            // --- KURAL 2: OTOMATİK SİPARİŞ (5'in altı) ---
            if (product.StockQuantity < 5)
            {
                System.Console.WriteLine($"[ACİL] '{product.Name}' stoğu KRİTİK SEVİYEDE! ({product.StockQuantity}). Otomatik sipariş geçiliyor...");

                // Stok azaldığı için fiyatı artır (Eski kuralımız)
                product.Price = product.Price * 1.20m; // %20 Zam (Kriz zammı)

                // Tedarikçiden 100 tane sipariş et
                await _supplierService.OrderStockAsync(product, 100);
            }

            // İsim düzeltme kuralı
            if (!string.IsNullOrEmpty(product.Name))
            {
                product.Name = product.Name.Trim().ToUpper();
            }

            // Ürünü yayınla (Veritabanına kaydet)
            await _publisher.PublishProductAsync(product);
        }
    }
}