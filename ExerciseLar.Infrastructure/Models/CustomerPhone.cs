using ExerciseLar.Enums;

namespace ExerciseLar.Infrastructure.Models
{
	public class CustomerPhone
	{
		public long CustomerPhoneID { get; set; }
		public long CustomerID { get; set; }
		public PhoneType Type { get; set; }
		public string? Number { get; set; }

		public DateTimeOffset CreatedOn { get; set; }
		public DateTimeOffset? LastModifiedOn { get; set; }
		public string? SearchTerms { get; set; }

		public string BuildSearchTerms() => $"{CustomerPhoneID} {Number} {SearchTerms}".ToLower();

		public virtual Customer? Customer { get; set; }
	}
}
