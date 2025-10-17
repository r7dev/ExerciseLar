using ExerciseLar.Enums;

namespace ExerciseLar.DTOs
{
	public class CustomerPhoneDto
	{
		public long CustomerPhoneID { get; set; }
		public long CustomerID { get; set; }
		public PhoneType Type { get; set; }
		public string? Number { get; set; }

		public DateTimeOffset CreatedOn { get; set; }
		public DateTimeOffset? LastModifiedOn { get; set; }
	}
}
