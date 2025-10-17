using ExerciseLar.Enums;

namespace ExerciseLar.FoundationAPI.Services
{
	public interface ISettingsService
	{
		DataProviderType DataProvider { get; set; }
		string SQLServerConnectionString { get; }
	}
}
