namespace WebApplication1.Models
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public string Brand { get; set; }
		public string Category { get; set; }
		public string Seller { get; set; }
		public float Price { get; set; }
		public string ImagePath { get; set; }

	}
}
