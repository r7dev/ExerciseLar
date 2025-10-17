namespace ExerciseLar.DTOs
{
	public class CustomerResponse
	{
		public long CustomerID { get; set; }
		public string? FullName { get; set; }
		public string? DocumentNumber { get; set; }
		public DateTime? DateOfBirth { get; set; }
	}
}
