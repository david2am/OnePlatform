using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Api.Payments;

[ApiController]
[Route("api/payments")]
public sealed class PaymentsController(ISender sender) : ControllerBase
{

  [HttpPost]
  public async Task<IActionResult> Submit(
    [FromBody] ReceivePaymentRequest request,
    CancellationToken ct)
  {
    var commandResult = request.ToCommand();
    if (commandResult.IsFailure)
      return ToProblem(commandResult.Error);

    var result = await sender.Send(commandResult.Value, ct);
    return result.IsSuccess
      ? Ok(result.Value)
      : BadRequest(result.Error);
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