using ExerciseLar.FoundationAPI.Services;
using ExerciseLar.FoundationAPI.Services.DataServiceFactory;
using ExerciseLar.FoundationAPI.Services.Infrastructure;

namespace ExerciseLar.FoundationAPI.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection Configure(this IServiceCollection services)
		{
			services.AddSingleton<ISettingsService, SettingsService>();

			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IDataServiceFactory, DataServiceFactory>();
			services.AddScoped<ICustomerService, CustomerService>();
			services.AddScoped<ICustomerPhoneService, CustomerPhoneService>();
			services.AddScoped<ISecurityService, SecurityService>();

			return services;
		}
	}
}
