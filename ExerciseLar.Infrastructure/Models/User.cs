namespace ExerciseLar.Infrastructure.Models
{
	public class User
	{
		public long UserID { get; set; }
		public required string Email { get; set; }
		public required string Password { get; set; }
	}
}
