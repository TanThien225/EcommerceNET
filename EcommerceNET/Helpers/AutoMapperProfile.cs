using AutoMapper;
using EcommerceNET.Data;
using EcommerceNET.ViewModels;

namespace EcommerceNET.Helpers
{
	public class AutoMapperProfile: Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<RegisterVM, KhachHang>();
				//.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
				//.ReverseMap();
		}
	}
}
