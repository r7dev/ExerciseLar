using ExerciseLar.Infrastructure.Common;
using ExerciseLar.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseLar.Infrastructure.DataServices
{
	partial class DataServiceBase
	{
		public async Task<Customer?> GetCustomerAsync(long id, CancellationToken cancellationToken)
		{
			return await _loanDataSource.Customers
				.Where(r => r.CustomerID == id)
				.FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<IList<Customer>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request, CancellationToken cancellationToken)
		{
			IQueryable<Customer> items = GetCustomers(request);

			// Execute
			var records = await items.Skip(skip).Take(take)
				.Select(r => new Customer
				{
					CustomerID = r.CustomerID,
					FirstName = r.FirstName,
					MiddleName = r.MiddleName,
					LastName = r.LastName,
					DocumentNumber = r.DocumentNumber,
					DateOfBirth = r.DateOfBirth,
				})
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			return records;
		}

		public async Task<IList<Customer>> GetCustomerKeysAsync(int skip, int take, DataRequest<Customer> request)
		{
			IQueryable<Customer> items = GetCustomers(request);

			// Execute
			var records = await items.Skip(skip).Take(take)
				.Select(r => new Customer
				{
					CustomerID = r.CustomerID,
				})
				.AsNoTracking()
				.ToListAsync();

			return records;
		}

		private IQueryable<Customer> GetCustomers(DataRequest<Customer> request, bool skipSorting = false)
		{
			IQueryable<Customer> items = _loanDataSource.Customers;

			// Query
			if (!String.IsNullOrEmpty(request.Query))
			{
				items = items.Where(r => EF.Functions.Like(r.SearchTerms, "%" + request.Query + "%"));
			}

			// Where
			if (request.Where != null)
			{
				items = items.Where(request.Where);
			}

			// Order By
			if (!skipSorting && request.OrderBys.Count != 0)
			{
				bool first = true;
				foreach (var (keySelector, desc) in request.OrderBys)
				{
					if (first)
					{
						items = desc ? items.OrderByDescending(keySelector) : items.OrderBy(keySelector);
						first = false;
					}
					else
					{
						items = desc
							? ((IOrderedQueryable<Customer>)items).ThenByDescending(keySelector)
							: ((IOrderedQueryable<Customer>)items).ThenBy(keySelector);
					}
				}
			}

			return items;
		}

		public async Task<int> GetCustomersCountAsync(DataRequest<Customer> request)
		{
			return await GetCustomers(request, true)
				.AsNoTracking()
				.CountAsync();
		}

		public async Task<int> SaveCustomerAsync(Customer entity, CancellationToken cancellationToken)
		{
			if (entity.CustomerID > 0)
			{
				_loanDataSource.Entry(entity).State = EntityState.Modified;
			}
			else
			{
				entity.CustomerID = UIDGenerator.Next();
				entity.CreatedOn = DateTimeOffset.Now;
				_loanDataSource.Entry(entity).State = EntityState.Added;
			}
			entity.LastModifiedOn = DateTimeOffset.Now;
			entity.SearchTerms = entity.BuildSearchTerms();
			return await _loanDataSource.SaveChangesAsync(cancellationToken);
		}

		public async Task<int> DeleteCustomersAsync(CancellationToken cancellationToken, params Customer[] entities)
		{
			return await _loanDataSource.Customers
				.Where(r => entities.Contains(r))
				.ExecuteDeleteAsync(cancellationToken);
		}
	}
}
