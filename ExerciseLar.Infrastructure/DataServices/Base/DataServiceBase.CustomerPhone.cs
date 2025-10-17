using ExerciseLar.Infrastructure.Common;
using ExerciseLar.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseLar.Infrastructure.DataServices
{
	partial class DataServiceBase
	{
		public async Task<CustomerPhone?> GetCustomerPhoneAsync(long id, CancellationToken cancellationToken)
		{
			return await _loanDataSource.CustomerPhones
				.Where(r => r.CustomerPhoneID == id)
				.Include(r => r.Customer)
				.FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<IList<CustomerPhone>> GetCustomerPhonesAsync(int skip, int take, DataRequest<CustomerPhone> request, CancellationToken cancellationToken)
		{
			IQueryable<CustomerPhone> items = GetCustomerPhones(request);

			// Execute
			var records = await items.Skip(skip).Take(take)
				.Select(r => new CustomerPhone
				{
					CustomerPhoneID = r.CustomerPhoneID,
					CustomerID = r.CustomerID,
					Type = r.Type,
					Number = r.Number,
					Customer = r.Customer == null ? null : new Customer
					{
						CustomerID = r.Customer.CustomerID,
						FirstName = r.Customer.FirstName,
						MiddleName = r.Customer.MiddleName,
						LastName = r.Customer.LastName,
						DocumentNumber = r.Customer.DocumentNumber
					}
				})
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			return records;
		}

		public async Task<IList<CustomerPhone>> GetCustomerPhoneKeysAsync(int skip, int take, DataRequest<CustomerPhone> request)
		{
			IQueryable<CustomerPhone> items = GetCustomerPhones(request);

			// Execute
			var records = await items.Skip(skip).Take(take)
				.Select(r => new CustomerPhone
				{
					CustomerPhoneID = r.CustomerPhoneID,
					CustomerID = r.CustomerID
				})
				.AsNoTracking()
				.ToListAsync();

			return records;
		}

		private IQueryable<CustomerPhone> GetCustomerPhones(DataRequest<CustomerPhone> request, bool skipSorting = false)
		{
			IQueryable<CustomerPhone> items = _loanDataSource.CustomerPhones;

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
							? ((IOrderedQueryable<CustomerPhone>)items).ThenByDescending(keySelector)
							: ((IOrderedQueryable<CustomerPhone>)items).ThenBy(keySelector);
					}
				}
			}

			return items;
		}

		public async Task<int> GetCustomerPhonesCountAsync(DataRequest<CustomerPhone> request)
		{
			return await GetCustomerPhones(request, true)
				.AsNoTracking()
				.CountAsync();
		}

		public async Task<int> SaveCustomerPhoneAsync(CustomerPhone entity, CancellationToken cancellationToken)
		{
			if (entity.CustomerPhoneID > 0)
			{
				_loanDataSource.Entry(entity).State = EntityState.Modified;
			}
			else
			{
				entity.CustomerPhoneID = UIDGenerator.Next();
				entity.CreatedOn = DateTimeOffset.Now;
				_loanDataSource.Entry(entity).State = EntityState.Added;
			}
			entity.LastModifiedOn = DateTimeOffset.Now;
			entity.SearchTerms = entity.BuildSearchTerms();
			return await _loanDataSource.SaveChangesAsync();
		}

		public async Task<int> DeleteCustomerPhonesAsync(CancellationToken cancellationToken, params CustomerPhone[] entities)
		{
			return await _loanDataSource.CustomerPhones
				.Where(r => entities.Contains(r))
				.ExecuteDeleteAsync();
		}
	}
}
