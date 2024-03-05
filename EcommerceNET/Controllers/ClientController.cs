using AutoMapper;
using EcommerceNET.Data;
using EcommerceNET.Helpers;
using EcommerceNET.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceNET.Controllers
{
	public class ClientController : Controller
	{
		private readonly BookshopContext db;
		private readonly IMapper _mapper;

		public ClientController(BookshopContext context, IMapper mapper)
		{
			db = context;
			_mapper = mapper;
		}

		#region Register


		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(RegisterVM model, IFormFile Image)
		{
			if (ModelState.IsValid)
			{
				try
				{
					//Map model RegisterVM sang -> database KHACHhang
					var client = _mapper.Map<KhachHang>(model);
					client.RandomKey = MyUtil.GenerateRandomKey();

					client.MatKhau = model.MatKhau.ToMd5Hash(client.RandomKey);

					client.HieuLuc = true;//Later email
					client.VaiTro = 0;

					if (Image != null)
					{
						client.Hinh = MyUtil.UploadImage(Image, "KhachHang");
					}

					db.Add(client);
					db.SaveChanges();
					return RedirectToAction("Index", "Home");
				}
				catch (Exception ex)
				{

				}
			}
			return View();
		}

		#endregion


		#region login

		[HttpGet]
		public IActionResult Login(string? ReturnUrl)
		{
			ViewBag.ReturnUrl = ReturnUrl;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
		{
			ViewBag.ReturnUrl = ReturnUrl;
			if (ModelState.IsValid)
			{
				var user = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.Username);
				//Khoong co user
				if (user == null)
				{
					ModelState.AddModelError("Error", "This user does not exist");
				}
				else
				{
					if (!user.HieuLuc)
					{
						ModelState.AddModelError("Error", "Your account has been locked. PLease contact with our service.");
					}
					else
					{
						//Check pass
						if (user.MatKhau != model.Password.ToMd5Hash(user.RandomKey))
						{
							ModelState.AddModelError("Error", "The info you've entered is incorrect.");
						}
						else
						{
							var claims = new List<Claim>
							{
								new Claim(ClaimTypes.Email,user.Email),
								new Claim(ClaimTypes.Name,user.HoTen),
								new Claim(mySetting.CLAIM_CUSTOMERID,user.MaKh),
								//Claim role động
								new Claim(ClaimTypes.Role, "Customer")
							};

							var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
							var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

							await HttpContext.SignInAsync(claimsPrincipal);

							if (Url.IsLocalUrl(ReturnUrl))
							{
								return Redirect(ReturnUrl);
							}
							else
							{
								return Redirect("/");
							}
						}
					}
				}
			}
			return View();
		}

		#endregion

		[Authorize]
		public IActionResult Profile()
		{
			return View();
		}


		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}
	}
}
