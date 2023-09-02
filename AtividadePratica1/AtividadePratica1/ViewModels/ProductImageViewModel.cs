namespace AtividadePratica1
{
    public class ProductImageViewModel
    {
        public Guid Id { get; set; }
        public Byte[] Image { get; set; }

        public ProductImageViewModel(Guid Id, Byte[] Image) { 
            this.Id = Id;
            this.Image = Image;
        }
    }
}
