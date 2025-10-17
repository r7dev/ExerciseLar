using ExerciseLar.FoundationAPI.Services.DataServiceFactory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExerciseLar.FoundationAPI.Services
{
	public class AuthService(IDataServiceFactory dataServiceFactory,
		ISecurityService _securityService,
		IConfiguration configuration) : IAuthService
	{
		private readonly IDataServiceFactory _dataServiceFactory = dataServiceFactory;
		private readonly ISecurityService _securityService = _securityService;
		private readonly IConfiguration _configuration = configuration;

		public async Task<string> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				throw new UnauthorizedAccessException("Username or password cannot be empty.");
			}

			var dataService = _dataServiceFactory.CreateDataService();
			var user = await dataService.GetUserByEmailAsync(username, cancellationToken) ?? throw new UnauthorizedAccessException("User not found.");
			if (!_securityService.VerifyHashedPassword(user.Password, password))
			{
				throw new UnauthorizedAccessException("Invalid username or password.");
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			string jwtSecret = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
			var key = Encoding.UTF8.GetBytes(jwtSecret);

			string jwtExpiration = _configuration["Jwt:ExpirationMinutes"] ?? throw new InvalidOperationException("JWT Expiration is not configured.");
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, user.Email)]),
				Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtExpiration)),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
