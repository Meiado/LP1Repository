namespace AtividadePratica1.ViewModels
{
    public class ProductGetAvailableViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public ProductGetAvailableViewModel(Guid Id, string Name, double Price) { 
            this.Id = Id;
            this.Name = Name;
            this.Price = Price;
        }
    }
}