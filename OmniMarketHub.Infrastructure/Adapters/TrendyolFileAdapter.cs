using System;
using System.IO;
using System.Text.Json; // JSON formatına çevirmek için gerekli kütüphane
using System.Threading.Tasks;
using OmniMarketHub.Domain.Entities;
using OmniMarketHub.Domain.Ports;

namespace OmniMarketHub.Infrastructure.Adapters
{
    // Bu sınıf, Domain katmanındaki "IProductPublisher" arayüzünü uygular (implemente eder).
    // Yani: "Ben bir ürün yayınlayıcıyım" der.
    public class TrendyolFileAdapter : IProductPublisher
    {
        // Dosyaları kaydedeceğimiz klasör yolu.
        // Masaüstünde "MarketData" diye bir klasör oluşturup oraya atacağız.
        private readonly string _basePath;

        public TrendyolFileAdapter()
        {
            // Masaüstü yolunu bulur ve sonuna "MarketData/Trendyol" ekler.
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _basePath = Path.Combine(desktopPath, "MarketData", "Trendyol_Outbox");

            // Eğer klasör yoksa oluşturur (System.IO özelliği)
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task PublishProductAsync(Product product)
        {
            // 1. Ürünü Trendyol'un anlayacağı formata (JSON) çevir.
            // JsonSerializer, C# nesnesini yazıya (string) döker.
            string jsonContent = JsonSerializer.Serialize(product, new JsonSerializerOptions
            {
                WriteIndented = true // Dosya okunaklı olsun diye girintili yazar
            });

            // 2. Dosya ismini oluştur (Örn: Trendyol_iPhone15_12345.json)
            string fileName = $"Trendyol_{product.Name}_{product.Id}.json";
            string fullPath = Path.Combine(_basePath, fileName);

            // 3. Dosyayı diske yaz (Simülasyon işlemi burasıdır)
            await File.WriteAllTextAsync(fullPath, jsonContent);

            // Konsola bilgi verelim (API çalışınca göreceğiz)
            Console.WriteLine($"[TrendyolAdapter] Ürün dosyası oluşturuldu: {fullPath}");
        }
    }
}