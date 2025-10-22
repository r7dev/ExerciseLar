using ExerciseLar.DTOs;
using ExerciseLar.FoundationAPI.Services;
using ExerciseLar.Infrastructure.Common;
using ExerciseLar.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseLar.FoundationAPI.Controllers
{
	[Route("api/customers/{customerID:long}/phones")]
	[ApiController]
	public class CustomerPhonesController(ICustomerPhoneService customerPhoneService) : ControllerBase
	{
		private readonly ICustomerPhoneService _customerPhoneService = customerPhoneService;

		[Authorize]
		[HttpGet()]
		public async Task<ActionResult<List<CustomerPhoneResponse>>> GetCustomerPhonesAsync(long customerID, [FromQuery] DefaultRequest request, CancellationToken cancellationToken)
		{
			var dataRequest = new DataRequest<CustomerPhone>()
			{
				Query = request.Query,
				Where = r => r.CustomerID == customerID,
				OrderBys =
				[
					(r => r.Type, false),
					(r => r.Number ?? string.Empty, false)
				]
			};
			var customerPhones = await _customerPhoneService.GetCustomerPhonesAsync(request.Skip, request.Take, dataRequest, cancellationToken);
			if (customerPhones is null || !customerPhones.Any())
				return Ok(new List<CustomerPhoneResponse>());

			List<CustomerPhoneResponse> response = [.. customerPhones.Select(c => CreateCustomerPhoneResponse(c))];
			return Ok(response);
		}

		[Authorize]
		[HttpGet("{id:long}")]
		public async Task<ActionResult<CustomerPhoneDetailsResponse>> GetCustomerPhoneByCustomerIdAsync(long customerID, long id, CancellationToken cancellationToken)
		{
			var item = await _customerPhoneService.GetCustomerPhoneAsync(id, cancellationToken);
			if (item is null || item.CustomerID != customerID)
				return NotFound();

			return Ok(CreateCustomerPhoneDetailsResponse(item));
		}

		[Authorize]
		[HttpPost()]
		public async Task<ActionResult<CustomerPhoneDetailsResponse>> CreateCustomerPhoneAsync(long customerID, [FromBody] CustomerPhoneDto model, CancellationToken cancellationToken)
		{
			try
			{
				model.CustomerID = customerID;
				long id = await _customerPhoneService.SaveCustomerPhoneAsync(model, cancellationToken);
				var item = await _customerPhoneService.GetCustomerPhoneAsync(id, cancellationToken);
				if (item is null)
					return NotFound();

				string actionName = nameof(GetCustomerPhoneByCustomerIdAsync).Replace("Async", "");
				return CreatedAtAction(actionName, new { customerID, id = item.CustomerPhoneID }, CreateCustomerPhoneDetailsResponse(item));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500, "An error occurred while creating the customer's phone number.");
			}
		}

		[Authorize]
		[HttpPut("{id:long}")]
		public async Task<ActionResult<CustomerPhoneDetailsResponse>> UpdateCustomerPhoneAsync(long customerID, long id, [FromBody] CustomerPhoneDto model, CancellationToken cancellationToken)
		{
			try
			{
				if (model is null)
					return BadRequest("Customer phone data is required.");

				model.CustomerPhoneID = id;
				model.CustomerID = customerID;
				await _customerPhoneService.SaveCustomerPhoneAsync(model, cancellationToken);
				var item = await _customerPhoneService.GetCustomerPhoneAsync(id, cancellationToken);
				if (item is null)
					return NotFound();

				return Ok(CreateCustomerPhoneDetailsResponse(item));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500, "An error occurred while updating the customer's phone number.");
			}
		}

		[Authorize]
		[HttpDelete("{id:long}")]
		public async Task<IActionResult> DeleteCustomerPhoneAsync(long id, CancellationToken cancellationToken)
		{
			try
			{
				await _customerPhoneService.DeleteCustomerPhoneAsync(new CustomerPhoneDto { CustomerPhoneID = id }, cancellationToken);

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500, "An error occurred while deleting the customer's phone number.");
			}
		}

		private static CustomerPhoneResponse CreateCustomerPhoneResponse(CustomerPhoneDto source)
		{
			return new CustomerPhoneResponse
			{
				CustomerPhoneID = source.CustomerPhoneID,
				CustomerID = source.CustomerID,
				Type = source.Type,
				Number = source.Number
			};
		}

		private static CustomerPhoneDetailsResponse CreateCustomerPhoneDetailsResponse(CustomerPhoneDto source)
		{
			return new CustomerPhoneDetailsResponse
			{
				CustomerPhoneID = source.CustomerPhoneID,
				CustomerID = source.CustomerID,
				Type = source.Type,
				Number = source.Number,
				CreatedOn = source.CreatedOn,
				LastModifiedOn = source.LastModifiedOn
			};
		}
	}
}
