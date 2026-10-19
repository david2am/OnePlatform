using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/payments")]
public sealed class PaymentsController : ControllerBase
{
  [HttpPost]
  public async Task<string> Submit()
  {
    return "dumb message";
  }
}