using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Api.Institutions;

[ApiController]
[Route("api/institutions")]
public sealed class InstitutionsController(ISender sender) : ControllerBase
{
  [HttpPost]
  public async Task<IActionResult> Create(
    [FromBody] CreateInstitutionRequest request,
    CancellationToken ct
  )
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