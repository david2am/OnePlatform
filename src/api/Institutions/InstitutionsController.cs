using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnePlatform.Clearing.Application.Institutions.Queries;
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

  [HttpGet("{routingNumber}")]
  public async Task<IActionResult> GetByRoutingNumber(
    string routingNumber,
    CancellationToken ct
  )
  {
    var query =new GetInstitutionByRoutingNumberQuery(routingNumber);

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