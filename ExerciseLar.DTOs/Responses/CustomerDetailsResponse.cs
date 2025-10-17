namespace ExerciseLar.DTOs
{
	public class CustomerDetailsResponse : CustomerResponse
	{
		public bool IsActive { get; set; }

		public DateTimeOffset? CreatedOn { get; set; }
		public DateTimeOffset? LastModifiedOn { get; set; }
	}
}
