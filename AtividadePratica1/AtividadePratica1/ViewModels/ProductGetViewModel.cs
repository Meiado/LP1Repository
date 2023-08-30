namespace AtividadePratica1.ViewModels
{
    public class ProductGetViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }

        public ProductGetViewModel(){

        }
        
        public ProductGetViewModel(string name, double price, int stockQuantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
        }
    }
}