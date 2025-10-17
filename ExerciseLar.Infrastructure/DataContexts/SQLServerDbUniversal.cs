using ExerciseLar.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseLar.Infrastructure.DataContexts
{
	public class SQLServerDbUniversal(string connectionString) : SQLServerDbBase(connectionString, schema), IUniversalDataSource
	{
		private const string schema = "Universal";

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable(nameof(User), schema);
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<User> Users { get; set; }
	}
}
