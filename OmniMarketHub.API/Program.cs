using Microsoft.EntityFrameworkCore;
using OmniMarketHub.Domain.Ports;
using OmniMarketHub.Infrastructure.Adapters;
using OmniMarketHub.Infrastructure.Persistence;
using OmniMarketHub.Domain.DomainServices;

var builder = WebApplication.CreateBuilder(args);

// --- BAĞLANTILAR (Dependency Injection) ---

// 1. Veritabanı Ayarı (RAM Veritabanı)
builder.Services.AddDbContext<OmniMarketDbContext>(options =>
    options.UseInMemoryDatabase("MarketDb"));


// 2. ADAPTÖR FİŞLERİ (Tek tek sisteme tanıtıyoruz)
// Dikkat: Buraya "IProductPublisher" demiyoruz, kendi isimleriyle ekliyoruz ki
// sistem bunları "Tek Yayıncı" sanıp hemen kullanmaya kalkmasın.
builder.Services.AddScoped<AmazonFileAdapter>();
builder.Services.AddScoped<TrendyolFileAdapter>();
builder.Services.AddScoped<PostgresProductAdapter>();

// 3. TEDARİKÇİ SERVİSİ
builder.Services.AddScoped<ISupplierService, SupplierFileAdapter>();


// 4. ÇOĞALTICI (Broadcaster) ve MANAGER AYARI
// Burası sihrin olduğu yer. Manager "Bana yayıncı ver" dediğinde, ona "Çoğaltıcıyı" veriyoruz.
builder.Services.AddScoped<ProductManager>(provider =>
{
    // Sistemdeki adaptörleri elle topluyoruz
    var amazon = provider.GetRequiredService<AmazonFileAdapter>();
    var trendyol = provider.GetRequiredService<TrendyolFileAdapter>();
    var db = provider.GetRequiredService<PostgresProductAdapter>();

    // Hepsini bir listeye koyuyoruz
    var tumKanallar = new List<IProductPublisher> { amazon, trendyol, db };

    // Çoğaltıcıyı oluşturup listeyi içine atıyoruz
    var broadcaster = new OmniChannelBroadcaster(tumKanallar);

    // Tedarikçi servisini de alıyoruz
    var supplier = provider.GetRequiredService<ISupplierService>();

    // Manager'ı bu süper güçlerle başlatıyoruz
    return new ProductManager(broadcaster, supplier);
});

// -------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();