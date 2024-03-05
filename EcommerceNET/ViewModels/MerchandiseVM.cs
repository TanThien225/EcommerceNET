namespace EcommerceNET.ViewModels
{
	public class MerchandiseVM
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public string Image { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		public string CategoryName { get; set; }
	}

    public class MerchandiseDetailVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string DetailDescription { get; set; }
        public int StarChecked { get; set; }
        public int QuantityLeft { get; set; }
    }

}
