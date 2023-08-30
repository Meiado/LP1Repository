using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.IO;
using AtividadePratica1.ViewModels;

namespace AtividadePratica1.Controllers 
{
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

        [HttpGet("{id}")]
        [Route("[action]")]
        public IActionResult Get(Guid id) 
        {
            string productsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.json");
            List<ProductGetViewModel> products = GetProducts();

            var product = JsonSerializer.Serialize(products.Where(p => p.Id == id).FirstOrDefault());

            return Content(product, "application/json");
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

        [HttpPut("{id}")]
        [Route("[action]")]
        public IActionResult Put(Guid id, ViewModels.ProductGetViewModel product) 
        {
            List<ProductGetViewModel> products = GetProducts();

            var productToUpdate = products.Where(p => p.Id == id).FirstOrDefault();

            if (productToUpdate != null)
            {
                productToUpdate.Name = product.Name;
                productToUpdate.Price = product.Price;
                productToUpdate.StockQuantity = product.StockQuantity;
            }

            WriteProducts(products);

            return Content(JsonSerializer.Serialize(productToUpdate), "application/json");   
        }

        [HttpDelete("{id}")]
        [Route("[action]")]
        public IActionResult Delete(Guid id) 
        {
            List<ProductGetViewModel> products = GetProducts();
            products.RemoveAll(p => p.Id == id);
            var json = new {Message = $"Produto {id} removido com sucesso"};
            WriteProducts(products);
            return Content(JsonSerializer.Serialize(json), "application/json");
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