using EcommerceNET.Data;
using EcommerceNET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceNET.ViewComponents
{
	public class MenuCategoryViewComponent : ViewComponent
	{
		private readonly BookshopContext DB;

		public MenuCategoryViewComponent(BookshopContext context) => DB = context;

		public IViewComponentResult Invoke()
		{
			var data = DB.Loais.Select(cate => new MenuCategoryVM
			{
				Maloai = cate.MaLoai,
				TenLoai = cate.TenLoai,
				Soluong = cate.HangHoas.Count
			}).OrderBy(p => p.TenLoai);

			return View(data); //Default.cshtml
			//return View("Default", data); 
		}
	}
}
