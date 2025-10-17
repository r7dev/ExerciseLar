namespace ExerciseLar.DTOs
{
	public class CustomerDto
	{
		public long CustomerID { get; set; }
		public string? FirstName { get; set; }
		public string? MiddleName { get; set; }
		public string? LastName { get; set; }
		public string? DocumentNumber { get; set; }
		public DateTime DateOfBirth { get; set; }
		public bool IsActive { get; set; }

		public string FullName => $"{FirstName} {MiddleName} {LastName}";

		public DateTimeOffset CreatedOn { get; set; }
		public DateTimeOffset? LastModifiedOn { get; set; }
	}
}
