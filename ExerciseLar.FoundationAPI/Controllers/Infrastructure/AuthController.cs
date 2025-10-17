using ExerciseLar.DTOs;
using ExerciseLar.FoundationAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseLar.FoundationAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController(IAuthService authService) : Controller
	{
		private readonly IAuthService _authService = authService;

		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync([FromBody] UserRequest user, CancellationToken cancellationToken)
		{
			try
			{
				var token = await _authService.AuthenticateAsync(user.Username, user.Password, cancellationToken);
				if (token == null)
					return Unauthorized();

				return Ok(new { Token = token });
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
