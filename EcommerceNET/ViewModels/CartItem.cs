namespace EcommerceNET.ViewModels
{
	public class CartItem
	{
		public int IdItem { get; set; }
		public string NameItem { get; set; }
		public string Image { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public double Total => Math.Round(Price * Quantity, 2);
	}
}
