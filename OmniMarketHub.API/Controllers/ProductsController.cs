using Microsoft.AspNetCore.Mvc;
using OmniMarketHub.Domain.Entities;
using OmniMarketHub.Domain.DomainServices; // Yeni beynimizi ekledik
using System.Threading.Tasks;

namespace OmniMarketHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // ARTIK DİREKT PUBLISHER (Yazıcı) YOK, MANAGER (Yönetici) VAR.
        private readonly ProductManager _manager;

        public ProductsController(ProductManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            // Gelen isteği direkt yöneticiye iletiyoruz.
            // Zam yapılıp yapılmayacağına o karar verecek.
            await _manager.ProcessProductAsync(product);

            return Ok(new
            {
                message = "Ürün iş mantığından geçti ve yayınlandı!",
                finalPrice = product.Price, // Son fiyatı ekranda görelim
                productName = product.Name
            });
        }
    }
}