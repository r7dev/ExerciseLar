using ExerciseLar.DTOs;
using ExerciseLar.FoundationAPI.Services.DataServiceFactory;
using ExerciseLar.Infrastructure.Common;
using ExerciseLar.Infrastructure.DataServices;
using ExerciseLar.Infrastructure.Models;

namespace ExerciseLar.FoundationAPI.Services
{
	public class CustomerPhoneService(IDataServiceFactory dataServiceFactory) : ICustomerPhoneService
	{
		private readonly IDataServiceFactory _dataServiceFactory = dataServiceFactory;

		public async Task<CustomerPhoneDto?> GetCustomerPhoneAsync(long id, CancellationToken cancellationToken)
		{
			using var dataService = _dataServiceFactory.CreateDataService();
			return await GetCustomerPhoneAsync(dataService, id, cancellationToken);
		}

		private static async Task<CustomerPhoneDto?> GetCustomerPhoneAsync(IDataService dataService, long id, CancellationToken cancellationToken)
		{
			var item = await dataService.GetCustomerPhoneAsync(id, cancellationToken);
			if (item != null)
			{
				return await CreateCustomerPhoneDtoAsync(item, includeAllFields: true);
			}
			return null;
		}

		public async Task<IList<CustomerPhoneDto>> GetCustomerPhonesAsync(int skip, int take, DataRequest<CustomerPhone> request, CancellationToken cancellationToken)
		{
			var models = new List<CustomerPhoneDto>();
			using var dataService = _dataServiceFactory.CreateDataService();
			var items = await dataService.GetCustomerPhonesAsync(skip, take, request, cancellationToken);
			foreach (var item in items)
			{
				models.Add(await CreateCustomerPhoneDtoAsync(item, includeAllFields: true));
			}
			return models;
		}

		public async Task<long> SaveCustomerPhoneAsync(CustomerPhoneDto model, CancellationToken cancellationToken)
		{
			long id = model.CustomerPhoneID;
			using var dataService = _dataServiceFactory.CreateDataService();
			var item = id > 0
				? await dataService.GetCustomerPhoneAsync(model.CustomerPhoneID, cancellationToken)
				: new CustomerPhone() { };
			if (item != null)
			{
				UpdateCustomerPhoneFromDto(item, model);
				await dataService.SaveCustomerPhoneAsync(item, cancellationToken);
				return item.CustomerPhoneID;
			}
			return 0;
		}

		public async Task<int> DeleteCustomerPhoneAsync(CustomerPhoneDto model, CancellationToken cancellationToken)
		{
			var item = new CustomerPhone { CustomerPhoneID = model.CustomerPhoneID };
			using var dataService = _dataServiceFactory.CreateDataService();
			return await dataService.DeleteCustomerPhonesAsync(cancellationToken, item);
		}

		private static async Task<CustomerPhoneDto> CreateCustomerPhoneDtoAsync(CustomerPhone source, bool includeAllFields)
		{
			var model = new CustomerPhoneDto()
			{
				CustomerPhoneID = source.CustomerPhoneID,
				CustomerID = source.CustomerID,
				Number = source.Number,
				CreatedOn = source.CreatedOn,
				LastModifiedOn = source.LastModifiedOn,
			};
			if (includeAllFields)
			{
				model.Type = source.Type;
			}
			await Task.CompletedTask;
			return model;
		}

		private static void UpdateCustomerPhoneFromDto(CustomerPhone target, CustomerPhoneDto source)
		{
			target.CustomerID = source.CustomerID;
			target.Type = source.Type;
			target.Number = source.Number;
			target.CreatedOn = source.CreatedOn;
			target.LastModifiedOn = source.LastModifiedOn;
		}
	}
}
