using ExerciseLar.Enums;

namespace ExerciseLar.DTOs
{
	public class CustomerPhoneResponse
	{
		public long CustomerPhoneID { get; set; }
		public long CustomerID { get; set; }
		public PhoneType Type { get; set; }
		public string? Number { get; set; }
	}
}
