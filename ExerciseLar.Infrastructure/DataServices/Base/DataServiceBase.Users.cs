using ExerciseLar.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseLar.Infrastructure.DataServices
{
	partial class DataServiceBase
	{
		public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
		{
			return await _universalDataSource.Users
				.Where(u => u.Email == email)
				.FirstOrDefaultAsync(cancellationToken);
		}
	}
}
