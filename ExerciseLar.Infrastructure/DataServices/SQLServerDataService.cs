using ExerciseLar.Infrastructure.DataContexts;

namespace ExerciseLar.Infrastructure.DataServices
{
	public partial class SQLServerDataService(string connectionString)
		: DataServiceBase(new SQLServerDbLoan(connectionString))
	{
	}
}
