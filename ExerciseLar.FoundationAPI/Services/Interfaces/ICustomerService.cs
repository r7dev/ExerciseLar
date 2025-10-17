using ExerciseLar.DTOs;
using ExerciseLar.Infrastructure.Common;
using ExerciseLar.Infrastructure.Models;

namespace ExerciseLar.FoundationAPI.Services
{
	public interface ICustomerService
	{
		public Task<CustomerDto?> GetCustomerAsync(long id, CancellationToken cancellationToken);
		public Task<IList<CustomerDto>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request, CancellationToken cancellationToken);
		public Task<long> SaveCustomerAsync(CustomerDto model, CancellationToken cancellationToken);
		public Task<int> DeleteCustomerAsync(CustomerDto model, CancellationToken cancellationToken);
	}
}
