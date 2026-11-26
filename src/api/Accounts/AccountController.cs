using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnePlatform.Clearing.Accounts.Application.Queries;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Api.Accounts;

[ApiController]
[Route("api/accounts")]
public sealed class AccountController(ISender sender) : ControllerBase  
{
  [HttpPost]
  public async Task<IActionResult> Open(
    [FromBody] OpenAccountRequest request,
    CancellationToken ct
  )
  {
    var commandResult = request.ToCommand();

    if (commandResult.IsFailure)
      return ToProblem(commandResult.Error);

    var result = await sender.Send(commandResult.Value, ct);

    return result.IsSuccess
      ? Ok(result.Value)
      : BadRequest();
  }

  [HttpGet("id:guid")]
  public async Task<IActionResult> GetById(
    Guid id,
    CancellationToken ct
  )
  {
    var query = new GetAccountsByInstitutionIdQuery(id);

    var response = await sender.Send(query, ct);

    return response.IsSuccess
      ? Ok(response.Value)
      : NotFound(response.Error);
  }

  [HttpGet]
  public async Task<IActionResult> GetByInstitutionId(
    [FromQuery] Guid institutionId,
    CancellationToken ct
  )
  {
    var query = new GetAccountsByInstitutionIdQuery(institutionId);

    var response = await sender.Send(query, ct);

    return response.IsSuccess
      ? Ok(response.Value)
      : NotFound(response.Error);
  }
  private ObjectResult ToProblem(Error error) => Problem(
    title: error.Code,
    detail: error.Message,
    statusCode: error.Type switch
    {
        ErrorType.Validation => 422,
        ErrorType.Conflict => 409,
        ErrorType.NotFound => 404,
        _ => 500
    });
}
