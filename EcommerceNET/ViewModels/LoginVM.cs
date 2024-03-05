using System.ComponentModel.DataAnnotations;

namespace EcommerceNET.ViewModels
{
	public class LoginVM
	{
		[Display(Name ="Username")]
		[Required(ErrorMessage ="*")]
		[MaxLength(20, ErrorMessage = "Limit 20 characters.")]
		public string Username { get; set; }

		[Display(Name = "Password")]
		[Required(ErrorMessage = "*")]
		[DataType(DataType.Password)]
		public string Password { get; set; }	
	}
}
