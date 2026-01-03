using Moq; // Taklit kütüphanesi
using Xunit; // Test kütüphanesi
using OmniMarketHub.Domain.Entities;
using OmniMarketHub.Domain.Ports;
using OmniMarketHub.Domain.DomainServices;
using System.Threading.Tasks;

namespace OmniMarketHub.Tests
{
    public class ProductManagerTests
    {
        [Fact] // Bu bir testtir etiketi
        public async Task Stok_5ten_Az_Ise_Fiyata_Zam_Yapilmali_Ve_Siparis_Gecilmeli()
        {
            // 1. HAZIRLIK (Arrange)
            // Sahte (Mock) adaptörler oluþturuyoruz. Gerçek veritabanýna gitmesin diye.
            var sahteYayinci = new Mock<IProductPublisher>();
            var sahteTedarikci = new Mock<ISupplierService>();

            // Test edeceðimiz beyni (Manager) oluþturuyoruz
            var manager = new ProductManager(sahteYayinci.Object, sahteTedarikci.Object);

            // Test için bir ürün uyduruyoruz: Fiyatý 100 TL, Stoðu 3 (Yani kriz durumu!)
            var urun = new Product
            {
                Name = "Test Urunu",
                Price = 100,
                StockQuantity = 3
            };

            // 2. EYLEM (Act)
            // Metodu çalýþtýrýyoruz
            await manager.ProcessProductAsync(urun);

            // 3. DOÐRULAMA (Assert)

            // Kural: Fiyat %20 artmalýydý. 100 * 1.20 = 120 olmalý.
            Assert.Equal(120, urun.Price);

            // Kural: Tedarikçiye sipariþ verilmeli.
            // (Verify metodu: "Bu metod çaðrýldý mý?" diye kontrol eder)
            sahteTedarikci.Verify(x => x.OrderStockAsync(urun, 100), Times.Once);

            // Kural: Ürün yayýnlanmalý.
            sahteYayinci.Verify(x => x.PublishProductAsync(urun), Times.Once);
        }
    }
}