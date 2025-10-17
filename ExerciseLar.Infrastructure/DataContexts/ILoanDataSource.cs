using ExerciseLar.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExerciseLar.Infrastructure.DataContexts
{
	public interface ILoanDataSource : IDisposable
	{
		public DbSet<Customer> Customers { get; set; }
		public DbSet<CustomerPhone> CustomerPhones { get; set; }

		EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

		int SaveChanges();

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
