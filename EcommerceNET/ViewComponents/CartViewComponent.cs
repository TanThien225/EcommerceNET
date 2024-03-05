using EcommerceNET.Data;
using EcommerceNET.Helpers;
using EcommerceNET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceNET.ViewComponents
{
	public class CartViewComponent : ViewComponent
	{
		private readonly BookshopContext db;

		public CartViewComponent(BookshopContext context) {
			db = context;
		}

		public IViewComponentResult Invoke()
		{
			var Cart = HttpContext.Session.Get<List<CartItem>>(mySetting.CART_KEY) ?? new List<CartItem>();

			return View("CartPanel", new CartModel
			{
				Quantity = Cart.Sum(p => p.Quantity),
				Total = Cart.Sum(p => p.Total),
			});
		}
	}
}
