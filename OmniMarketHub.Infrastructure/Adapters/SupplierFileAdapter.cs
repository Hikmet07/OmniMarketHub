using System;
using System.IO;
using System.Threading.Tasks;
using OmniMarketHub.Domain.Entities;
using OmniMarketHub.Domain.Ports;

namespace OmniMarketHub.Infrastructure.Adapters
{
    public class SupplierFileAdapter : ISupplierService
    {
        private readonly string _basePath;

        public SupplierFileAdapter()
        {
            // Masaüstünde "MarketData/Tedarikci_Siparisleri" klasörü oluşturacağız
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _basePath = Path.Combine(desktopPath, "MarketData", "Tedarikci_Siparisleri");

            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task OrderStockAsync(Product product, int quantity)
        {
            // Sipariş fişi içeriği
            string orderContent = $@"
            === OTOMATİK SİPARİŞ FİŞİ ===
            Tarih: {DateTime.Now}
            Ürün ID: {product.Id}
            Ürün Adı: {product.Name}
            Mevcut Stok: {product.StockQuantity}
            -----------------------------
            İSTENEN MİKTAR: {quantity} ADET
            =============================
            ";

            string fileName = $"SIPARIS_{product.Name}_{Guid.NewGuid()}.txt";
            string fullPath = Path.Combine(_basePath, fileName);

            await File.WriteAllTextAsync(fullPath, orderContent);

            Console.WriteLine($"[SUPPLIER] Stok kritik! {product.Name} için {quantity} adet sipariş dosyası oluşturuldu.");
        }
    }
}