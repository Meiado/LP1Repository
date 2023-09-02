using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.IO;
using AtividadePratica1.ViewModels;

namespace AtividadePratica1.Controllers
{
    //Falta upload e download de imagem

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        [HttpGet]
        [Route("[action]")]
        public IActionResult Get()
        {
            List<ProductGetViewModel> products = GetProducts();

            var json = JsonSerializer.Serialize(products);

            return Content(json, "application/json");
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult Get(Guid id)
        {
            List<ProductGetViewModel> products = GetProducts();
            var product = products.Where(p => p.Id == id).FirstOrDefault();
            if (product == null)
                return NotFound(
                    new
                    {
                        Message = $"Produto {id} não encontrado"
                    }
                );    
            var productJson = JsonSerializer.Serialize(product);
            return Content(productJson, "application/json"); 
            
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAvailables()
        {
            List<ProductGetViewModel> products = GetProducts();

            List<ProductGetAvailableViewModel> availables = GetAvailables(products);

            var json = JsonSerializer.Serialize(availables);

            return Content(json, "application/json");
        }

        [HttpGet]
        [Route("[action]/{name}")]
        public IActionResult GetByName(string name)
        {
            List<ProductGetViewModel> products = GetProducts();

            var product = products.Where(p => p.Name == name).FirstOrDefault();

            if(product == null)
                return NotFound(
                    new
                    {
                        Message = $"Produto {name} não encontrado"
                    }
                );
            var productJson = JsonSerializer.Serialize(product);
            return Content(productJson, "application/json");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Post(ViewModels.ProductGetViewModel data)
        {
            List<ProductGetViewModel> products = GetProducts();

            var product = new ProductGetViewModel(data.Name, data.Price, data.StockQuantity);

            products.Add(product);

            WriteProducts(products);

            return CreatedAtAction(nameof(Get), product);
        }

        [HttpPost]
        [Route("[action]/{id}/upload-image")]
        public async Task<IActionResult> UploadImage(Guid id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("O arquivo não foi fornecido ou está vazio.");
            }

            if (file.Length > 1 * 1024 * 1024) // 1 MB
            {
                return BadRequest("O tamanho do arquivo excede 1 MB.");
            }

            string uploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
            string extension = Path.GetExtension(file.FileName);
            string fileName = $"{id}{extension}";

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Agora você pode salvar o caminho do arquivo no objeto do produto ou onde preferir

            return Ok(new { Message = "Imagem enviada com sucesso." });
        }


        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult Put(Guid id, ViewModels.ProductGetViewModel product)
        {
            List<ProductGetViewModel> products = GetProducts();

            var productToUpdate = products.Where(p => p.Id == id).FirstOrDefault();

            if (productToUpdate != null)
            {
                productToUpdate.Name = product.Name;
                productToUpdate.Price = product.Price;
                productToUpdate.StockQuantity = product.StockQuantity;
                WriteProducts(products);

                return Content(JsonSerializer.Serialize(productToUpdate), "application/json");
            }

            return NotFound(
                new
                {
                    Message = $"Produto {id} não encontrado"
                }
            );
        }

        [HttpPut]
        [Route("[action]/{id}/{amount}")]
        public IActionResult Purchase(Guid id, int amount)
        {
            List<ProductGetViewModel> products = GetProducts();
            var productToUpdate = products.Where(p => p.Id == id).FirstOrDefault();
            if (productToUpdate != null && productToUpdate.StockQuantity - amount >= 0)
            {
                productToUpdate.StockQuantity -= amount;
                WriteProducts(products);
                return Content(JsonSerializer.Serialize(productToUpdate), "application/json");
            }
            if(productToUpdate.StockQuantity - amount < 0)
            {
                var msg = new { Message = "Product not available at desired amount." };
                return Content(JsonSerializer.Serialize(msg), "application/json");
            }
            return NotFound(
                new
                {
                    Message = $"Produto {id} não encontrado"
                }
            );

        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            List<ProductGetViewModel> products = GetProducts();
            products.RemoveAll(p => p.Id == id);
            var json = new {Message = $"Produto {id} removido com sucesso"};
            WriteProducts(products);
            return Content(JsonSerializer.Serialize(json), "application/json");
        }

        private List<ProductGetAvailableViewModel> GetAvailables(List<ProductGetViewModel> products)
        {

            List<ProductGetAvailableViewModel> availables = new();
            foreach (var item in products)
            {
                if (item.StockQuantity > 0)
                    availables.Add(new(item.Id, item.Name, item.Price));
            }

            return availables;
        }

        private List<ProductGetViewModel> GetProducts()
        {
            string productsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.json");

            if (System.IO.File.Exists(productsPath))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var productsJson = System.IO.File.ReadAllText(productsPath);
                List<ProductGetViewModel> products = System.Text.Json.JsonSerializer.Deserialize<List<ProductGetViewModel>>(productsJson, options);
                return products;
            }
            return null;
        }

        private void WriteProducts(List<ProductGetViewModel> products)
        {
            string productsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.json");

            var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

            System.IO.File.WriteAllText(productsPath, JsonSerializer.Serialize(products, options));
        }
    }
}
