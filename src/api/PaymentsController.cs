using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/payments")]
public sealed class PaymentsController : ControllerBase
{
  [HttpGet]
  public async Task<string> Submit()
  {
    return "dumb message";
  }
}