using ExerciseLar.Infrastructure.DataServices;

namespace ExerciseLar.FoundationAPI.Services.DataServiceFactory
{
	public interface IDataServiceFactory
	{
		IDataService CreateDataService();
	}
}
