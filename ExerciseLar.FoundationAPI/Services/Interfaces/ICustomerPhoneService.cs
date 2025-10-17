using ExerciseLar.DTOs;
using ExerciseLar.Infrastructure.Common;
using ExerciseLar.Infrastructure.Models;

namespace ExerciseLar.FoundationAPI.Services
{
	public interface ICustomerPhoneService
	{
		public Task<CustomerPhoneDto?> GetCustomerPhoneAsync(long id, CancellationToken cancellationToken);
		public Task<IList<CustomerPhoneDto>> GetCustomerPhonesAsync(int skip, int take, DataRequest<CustomerPhone> request, CancellationToken cancellationToken);
		public Task<long> SaveCustomerPhoneAsync(CustomerPhoneDto model, CancellationToken cancellationToken);
		Task<int> DeleteCustomerPhoneAsync(CustomerPhoneDto model, CancellationToken cancellationToken);
	}
}
