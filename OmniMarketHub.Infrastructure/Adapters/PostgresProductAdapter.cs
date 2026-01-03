using System;
using System.Threading.Tasks;
using OmniMarketHub.Domain.Entities;
using OmniMarketHub.Domain.Ports;
using Microsoft.EntityFrameworkCore;
using OmniMarketHub.Infrastructure.Persistence;

namespace OmniMarketHub.Infrastructure.Adapters
{
    // Bu adaptör de bir "Yayıncıdır". Ama dosyaya değil, veritabanına yazar.
    public class PostgresProductAdapter : IProductPublisher
    {
        private readonly OmniMarketDbContext _context;

        public PostgresProductAdapter(OmniMarketDbContext context)
        {
            _context = context;
        }

        public async Task PublishProductAsync(Product product)
        {
            // 1. Ürünü veritabanı listesine ekle
            await _context.Products.AddAsync(product);

            // 2. Değişiklikleri kaydet (SQL Insert komutu burada çalışır)
            await _context.SaveChangesAsync();

            Console.WriteLine($"[Database] Ürün veritabanına kaydedildi! ID: {product.Id}");
        }
    }
}