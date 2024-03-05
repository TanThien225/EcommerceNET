using EcommerceNET.Data;
using EcommerceNET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EcommerceNET.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceNET.Controllers
{
	public class CartController : Controller
	{
		private readonly BookshopContext DB;
		public CartController(BookshopContext context)
		{
			DB = context;
		}

		public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(mySetting.CART_KEY) ?? new List<CartItem>();

		public IActionResult Index()
		{
			return View(Cart);
		}

		public IActionResult AddToCart(int id, int quantity = 1)
		{
			var shoppingCart = Cart;
			var item = shoppingCart.SingleOrDefault(i => i.IdItem == id);

			//Chua co san pham trong gio hang
			if (item == null)
			{
				var product = DB.HangHoas.SingleOrDefault(p => p.MaHh == id);
				if (product == null)
				{
					TempData["Message"] = $"Does not exist product {id} in our website!";
					return Redirect("/404");
				}
				//tao moi san Pham
				item = new CartItem
				{
					IdItem = product.MaHh,
					NameItem = product.TenHh,
					Image = product.Hinh ?? String.Empty,
					Price = product.DonGia ?? 0,
					Quantity = quantity,
				};
				//Theme vao gio hang
				shoppingCart.Add(item);
			}
			else
			{
				//Tang so luong len
				item.Quantity += quantity;
			}

			//Them vao session
			HttpContext.Session.Set(mySetting.CART_KEY, shoppingCart);
			return RedirectToAction("Index");
		}

		public IActionResult RemoveCart(int id)
		{
			var shoppingCart = Cart;
			var item = shoppingCart.SingleOrDefault(i => i.IdItem == id);
			if (item != null)
			{
				shoppingCart.Remove(item);
				HttpContext.Session.Set(mySetting.CART_KEY, shoppingCart);
			}
			return RedirectToAction("Index");
		}

		[Authorize]
		[HttpGet]
		public IActionResult Checkout()
		{
			if (Cart.Count == 0)
			{
				return RedirectToAction("HangHoa", "Index");
			}
			return View(Cart);
		}

		[Authorize]
		[HttpPost]
		public IActionResult Checkout(CheckOutVM model)
		{
			if (ModelState.IsValid)
			{
				var customerId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == mySetting.CLAIM_CUSTOMERID)?.Value;

				//neu tick vao giong du lieu, lay du lieu tu data
				var Customer = new KhachHang();
				if (model.SameWithUser)
				{
					Customer = DB.KhachHangs.SingleOrDefault(c => c.MaKh == customerId);
				}

				//add vao db
				var Payment = new HoaDon
				{
					MaKh = customerId,
					HoTen = model.ReceiverName ?? Customer.HoTen,
					DiaChi = model.Address ?? Customer.DiaChi,
					DienThoai = model.PhoneNumber ?? Customer.DienThoai,
					NgayDat = DateTime.Now,
					CachThanhToan = "COD",
					CachVanChuyen = "GHN",
					MaTrangThai = 0,
					GhiChu = model.NotePurchase,
				};


				//Them tiep theo vao chi tiet hoa don
				DB.Database.BeginTransaction();
				try
				{
					DB.Database.CommitTransaction();
					DB.Add(Payment);
					DB.SaveChanges();

					var detailPayments = new List<ChiTietHd>();
					foreach (var item in Cart)
					{
						detailPayments.Add(new ChiTietHd
						{
							MaHd = Payment.MaHd,
							SoLuong = item.Quantity,
							DonGia = item.Price,
							MaHh = item.IdItem,
							GiamGia = 0,
						});
					}
					DB.AddRange(detailPayments);	
					DB.SaveChanges();
					//make cart empty
					HttpContext.Session.Set<List<CartItem>>(mySetting.CART_KEY, new List<CartItem>());
					//success
					return View("Success");
				}
				catch
				{
					DB.Database.RollbackTransaction();
				}
			}
			return View(Cart);
		}
	}
}
