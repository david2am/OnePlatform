using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnePlatform.Clearing.Domain.Shared;

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
