using ExerciseLar.DTOs;
using ExerciseLar.FoundationAPI.Services.DataServiceFactory;
using ExerciseLar.Infrastructure.Common;
using ExerciseLar.Infrastructure.DataServices;
using ExerciseLar.Infrastructure.Models;

namespace ExerciseLar.FoundationAPI.Services
{
	public class CustomerService(IDataServiceFactory dataServiceFactory) : ICustomerService
	{
		private readonly IDataServiceFactory _dataServiceFactory = dataServiceFactory;

		public async Task<CustomerDto?> GetCustomerAsync(long id, CancellationToken cancellationToken)
		{
			using var dataService = _dataServiceFactory.CreateDataService();
			return await GetCustomerAsync(dataService, id, cancellationToken);
		}

		private static async Task<CustomerDto?> GetCustomerAsync(IDataService dataService, long id, CancellationToken cancellationToken)
		{
			var item = await dataService.GetCustomerAsync(id, cancellationToken);
			if (item != null)
			{
				return await CreateCustomerDtoAsync(item, includeAllFields: true);
			}
			return null;
		}

		public async Task<IList<CustomerDto>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request, CancellationToken cancellationToken)
		{
			var models = new List<CustomerDto>();
			using var dataService = _dataServiceFactory.CreateDataService();
			var items = await dataService.GetCustomersAsync(skip, take, request, cancellationToken);
			foreach (var item in items)
			{
				models.Add(await CreateCustomerDtoAsync(item, includeAllFields: true));
			}
			return models;
		}

		public async Task<long> SaveCustomerAsync(CustomerDto model, CancellationToken cancellationToken)
		{
			long id = model.CustomerID;
			using var dataService = _dataServiceFactory.CreateDataService();
			var item = id > 0
				? await dataService.GetCustomerAsync(model.CustomerID, cancellationToken)
				: new Customer() {};
			if (item != null)
			{
				UpdateCustomerFromDto(item, model);
				await dataService.SaveCustomerAsync(item, cancellationToken);
				return item.CustomerID;
			}
			return 0;
		}

		public async Task<int> DeleteCustomerAsync(CustomerDto model, CancellationToken cancellationToken)
		{
			var item = new Customer { CustomerID = model.CustomerID };
			using var dataService = _dataServiceFactory.CreateDataService();
			return await dataService.DeleteCustomersAsync(cancellationToken, item);
		}

		private static async Task<CustomerDto> CreateCustomerDtoAsync(Customer source, bool includeAllFields)
		{
			var model = new CustomerDto()
			{
				CustomerID = source.CustomerID,
				FirstName = source.FirstName,
				MiddleName = source.MiddleName,
				LastName = source.LastName,
				DateOfBirth = source.DateOfBirth,
				CreatedOn = source.CreatedOn,
				LastModifiedOn = source.LastModifiedOn,
			};
			if (includeAllFields)
			{
				model.DocumentNumber = source.DocumentNumber;
				model.IsActive = source.IsActive;
			}
			await Task.CompletedTask;
			return model;
		}

		private static void UpdateCustomerFromDto(Customer target, CustomerDto source)
		{
			target.FirstName = source.FirstName;
			target.MiddleName = source.MiddleName;
			target.LastName = source.LastName;
			target.DocumentNumber = source.DocumentNumber;
			target.DateOfBirth = source.DateOfBirth;
			target.IsActive = source.IsActive;
			target.CreatedOn = source.CreatedOn;
			target.LastModifiedOn = source.LastModifiedOn;
		}
	}
}
