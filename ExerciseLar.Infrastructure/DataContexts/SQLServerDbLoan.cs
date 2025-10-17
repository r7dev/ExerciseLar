using ExerciseLar.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseLar.Infrastructure.DataContexts
{
	public partial class SQLServerDbLoan(string connectionString) : SQLServerDbBase(connectionString, schema), ILoanDataSource
	{
		private const string schema = "Loan";

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Customer>().ToTable(nameof(Customer), schema);
			modelBuilder.Entity<CustomerPhone>().ToTable(nameof(CustomerPhone), schema);
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Customer> Customers { get; set; }
		public DbSet<CustomerPhone> CustomerPhones { get; set; }
	}
}
