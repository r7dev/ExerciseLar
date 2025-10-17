using ExerciseLar.Infrastructure.Common;
using ExerciseLar.Infrastructure.Models;

namespace ExerciseLar.Infrastructure.DataServices
{
	public interface IDataService : IDisposable
	{
		#region Schema Loan
		Task<Customer?> GetCustomerAsync(long id, CancellationToken cancellationToken);
		Task<IList<Customer>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request, CancellationToken cancellationToken);
		Task<IList<Customer>> GetCustomerKeysAsync(int skip, int take, DataRequest<Customer> request);
		Task<int> GetCustomersCountAsync(DataRequest<Customer> request);
		Task<int> SaveCustomerAsync(Customer entity, CancellationToken cancellationToken);
		Task<int> DeleteCustomersAsync(CancellationToken cancellationToken, params Customer[] entities);

		Task<CustomerPhone?> GetCustomerPhoneAsync(long id, CancellationToken cancellationToken);
		Task<IList<CustomerPhone>> GetCustomerPhonesAsync(int skip, int take, DataRequest<CustomerPhone> request, CancellationToken cancellationToken);
		Task<IList<CustomerPhone>> GetCustomerPhoneKeysAsync(int skip, int take, DataRequest<CustomerPhone> request);
		Task<int> GetCustomerPhonesCountAsync(DataRequest<CustomerPhone> request);
		Task<int> SaveCustomerPhoneAsync(CustomerPhone entity, CancellationToken cancellationToken);
		Task<int> DeleteCustomerPhonesAsync(CancellationToken cancellationToken, params CustomerPhone[] entities);
		#endregion
	}
}
