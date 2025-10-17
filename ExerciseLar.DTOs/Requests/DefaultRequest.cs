namespace ExerciseLar.DTOs
{
	public class DefaultRequest
	{
		public int Skip { get; set; }
		public int Take { get; set; }

		public string Query { get; set; } = string.Empty;
	}
}
