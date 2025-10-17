namespace ExerciseLar.Infrastructure.Models
{
	public class Customer
	{
		public long CustomerID { get; set; }
		public string? FirstName { get; set; }
		public string? MiddleName { get; set; }
		public string? LastName { get; set; }
		public string? DocumentNumber { get; set; }
		public DateTime DateOfBirth { get; set; }
		public bool IsActive { get; set; }

		public DateTimeOffset CreatedOn { get; set; }
		public DateTimeOffset? LastModifiedOn { get; set; }
		public string? SearchTerms { get; set; }

		public string BuildSearchTerms() => $"{CustomerID} {FirstName} {MiddleName} {LastName} {DocumentNumber}".ToLower();
	}
}
