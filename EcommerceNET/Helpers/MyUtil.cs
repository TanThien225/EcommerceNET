using System.Text;

namespace EcommerceNET.Helpers
{
	public class MyUtil
	{
		public static string GenerateRandomKey(int length = 5)
		{
			var pattern = @"qazwsxedcrfvtgbyhnujmiklopQAZWSXEDCRFVTGBYHNUJMIKLOP!@#$%^&*";
			var stringBuilder = new StringBuilder();
			var random = new Random(length);
			for (int i = 0; i < length; i++)
			{
				//Them vao stringbuilder 1 kitu trong pattern
				stringBuilder.Append(pattern[random.Next(0, pattern.Length)]);
			}
			return stringBuilder.ToString();
		}

		public static string UploadImage(IFormFile Image, string Folder)
		{
			try
			{
				var FullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", Folder, Image.FileName);
				using (var MyFile = new FileStream(FullPath, FileMode.CreateNew))
				{
					Image.CopyTo(MyFile);
				}
				return Image.FileName;
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}
	}
}
