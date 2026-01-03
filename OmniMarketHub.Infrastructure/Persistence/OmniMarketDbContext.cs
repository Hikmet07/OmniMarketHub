using Microsoft.EntityFrameworkCore;
using OmniMarketHub.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OmniMarketHub.Infrastructure.Persistence
{
    // Bu sınıf, kodumuzdaki nesneler ile veritabanındaki tabloları eşleştirir.
    public class OmniMarketDbContext : DbContext
    {
        public OmniMarketDbContext(DbContextOptions<OmniMarketDbContext> options) : base(options)
        {
        }

        // Domain katmanındaki "Product" nesnesi, veritabanında "Products" tablosu olacak.
        public DbSet<Product> Products { get; set; }

        // Tablo ayarlarını burada yapabiliriz
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Örneğin: Ürünün ID'si birincil anahtardır (Primary Key)
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            // Fiyat alanı veritabanında hassas tutulsun (Para birimi)
            

            base.OnModelCreating(modelBuilder);
        }
    }
}