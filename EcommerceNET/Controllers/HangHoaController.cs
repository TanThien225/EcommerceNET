using EcommerceNET.Data;
using EcommerceNET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceNET.Controllers
{
    public class HangHoaController : Controller
    {
		private readonly BookshopContext db;

		public HangHoaController(BookshopContext context) {
            db = context;
        }

        public IActionResult Index(int? loai)
        {
            var merchandise = db.HangHoas.AsQueryable();

            if (loai.HasValue)
            {
                merchandise = merchandise.Where(p => p.MaLoai == loai.Value);
			}

            var result = merchandise.Select(g => new MerchandiseVM
            {
                ItemId = g.MaHh,
                ItemName = g.TenHh,
                Price = g.DonGia ?? 0,
                Image = g.Hinh ?? "",
                Description = g.MoTaDonVi ?? "",
                CategoryName = g.MaLoaiNavigation.TenLoai,
            }); 

            return View(result);
        }

        public IActionResult Search(string? query)
        {
			var merchandise = db.HangHoas.AsQueryable();

			if (query != null)
			{
                merchandise = merchandise.Where(g => g.TenHh.Contains(query));
			}

			var result = merchandise.Select(g => new MerchandiseVM
			{
				ItemId = g.MaHh,
				ItemName = g.TenHh,
				Price = g.DonGia ?? 0,
				Image = g.Hinh ?? "",
				Description = g.MoTaDonVi ?? "",
				CategoryName = g.MaLoaiNavigation.TenLoai,
			});

			return View(result);
		}

        public IActionResult Detail(int id)
        {
            var data = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .SingleOrDefault(p => p.MaHh == id);
            if(data == null)
            {
                TempData["Message"] = $"Do not found any products have the same {id}";
                return Redirect("/404");
            }

            var result = new MerchandiseDetailVM
            {
                ItemId = data.MaHh,
                ItemName = data.TenHh,
                Price = data.DonGia ?? 0,
                Image = data.Hinh ?? "",
                Description = data.MoTaDonVi ?? "",
                DetailDescription = data.MoTa ?? "",
                CategoryName = data.MaLoaiNavigation.TenLoai,
                StarChecked = 5, //Check later
                QuantityLeft = 10,
            };

            return View(result);
        }
    }
}
