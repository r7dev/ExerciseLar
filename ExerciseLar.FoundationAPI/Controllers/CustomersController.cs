using ExerciseLar.DTOs;
using ExerciseLar.FoundationAPI.Services;
using ExerciseLar.Infrastructure.Common;
using ExerciseLar.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseLar.FoundationAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController(ICustomerService customerService) : ControllerBase
	{
		private readonly ICustomerService _customerService = customerService;

		[HttpGet()]
		public async Task<ActionResult<List<CustomerResponse>>> GetCustomersAsync(DefaultRequest request, CancellationToken cancellationToken)
		{
			var dataRequest = new DataRequest<Customer>()
			{
				Query = request.Query,
				OrderBys =
				[
					(r => r.FirstName ?? string.Empty, false),
					(r => r.LastName ?? string.Empty, false)
				]
			};
			var customers = await _customerService.GetCustomersAsync(request.Skip, request.Take, dataRequest, cancellationToken);
			if (customers is null || !customers.Any())
				return Ok(new List<CustomerResponse>());

			List<CustomerResponse> response = [.. customers.Select(c => CreateCustomerResponse(c))];
			return Ok(response);
		}

		[HttpGet("{id:long}")]
		public async Task<ActionResult<CustomerDetailsResponse>> GetCustomerByIdAsync(long id, CancellationToken cancellationToken)
		{
			var customer = await _customerService.GetCustomerAsync(id, cancellationToken);
			if (customer is null)
				return NotFound();
			return Ok(CreateCustomerDetailsResponse(customer));
		}

		[HttpPost()]
		public async Task<ActionResult<CustomerDetailsResponse>> CreateCustomerAsync([FromBody] CustomerDto model, CancellationToken cancellationToken)
		{
			try
			{
				long id = await _customerService.SaveCustomerAsync(model, cancellationToken);
				var item = await _customerService.GetCustomerAsync(id, cancellationToken);
				if (item is null)
					return NotFound();

				string actionName = nameof(GetCustomerByIdAsync).Replace("Async", "");
				return CreatedAtAction(actionName, new { id = item.CustomerID }, CreateCustomerDetailsResponse(item));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500, "An error occurred while creating the customer.");
			}
		}

		[HttpPut("{id:long}")]
		public async Task<ActionResult<CustomerDetailsResponse>> UpdateCustomerAsync(long id, [FromBody] CustomerDto model, CancellationToken cancellationToken)
		{
			try
			{
				if (model is null)
					return BadRequest("Customer data is required.");

				model.CustomerID = id;
				await _customerService.SaveCustomerAsync(model, cancellationToken);
				var item = await _customerService.GetCustomerAsync(id, cancellationToken);
				if (item is null)
					return NotFound();

				return Ok(CreateCustomerDetailsResponse(item));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500, "An error occurred while updating the customer.");
			}
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> DeleteCustomerAsync(long id, CancellationToken cancellationToken)
		{
			try
			{
				await _customerService.DeleteCustomerAsync(new CustomerDto { CustomerID = id }, cancellationToken);

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500, "An error occurred while deleting the customer.");
			}
		}

		private static CustomerResponse CreateCustomerResponse(CustomerDto source)
		{
			return new CustomerResponse
			{
				CustomerID = source.CustomerID,
				FullName = source.FullName,
				DocumentNumber = source.DocumentNumber,
				DateOfBirth = source.DateOfBirth
			};
		}

		private static CustomerDetailsResponse CreateCustomerDetailsResponse(CustomerDto source)
		{
			return new CustomerDetailsResponse
			{
				CustomerID = source.CustomerID,
				FullName = source.FullName,
				DocumentNumber = source.DocumentNumber,
				DateOfBirth = source.DateOfBirth,
				IsActive = source.IsActive,
				CreatedOn = source.CreatedOn,
				LastModifiedOn = source.LastModifiedOn
			};
		}
	}
}
