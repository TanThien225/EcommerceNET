using System.ComponentModel.DataAnnotations;

namespace EcommerceNET.ViewModels
{
	public class RegisterVM
	{
		[Display(Name = "Username")]
		[Required(ErrorMessage = "Enter your username")]
		[MaxLength(20, ErrorMessage = "Limit 20 characters.")]
		public string MaKh { get; set; }


		[Display(Name = "Password")]
		[Required(ErrorMessage = "Enter your password")]
		[DataType(DataType.Password)]
		public string MatKhau { get; set; }


		[Required(ErrorMessage = "*")]
		[MaxLength(50, ErrorMessage = "Limit 50 characters.")]
		public string HoTen { get; set; }

		public bool GioiTinh { get; set; } = true;

		[Display(Name = "Year Of Birth")]
		[DataType(DataType.Date)]
		public DateTime? NgaySinh { get; set; }

		[Display(Name = "Address")]
		[MaxLength(60, ErrorMessage = "Limit 60 characters.")]
		public string DiaChi { get; set; }

		[Display(Name = "Phone Number")]
		[MaxLength(24, ErrorMessage = "Limit 24 characters.")]
		[RegularExpression(@"0[9875]\d{8}", ErrorMessage = "Invalid Phone Number.")]
		public string DienThoai { get; set; }

		
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		public string? Hinh { get; set; }
	}
}
