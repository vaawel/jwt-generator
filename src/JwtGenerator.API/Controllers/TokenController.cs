using Asp.Versioning;
using JwtGenerator.API.Contract.Requests;
using JwtGenerator.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtGenerator.API.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/token")]
public class TokenController(TokenService tokenService) : ControllerBase
{
    private readonly TokenService _tokenService = tokenService;

    [HttpPost("generate")]
    public IActionResult GenerateToken([FromBody] GenerateTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.IdCompany.ToString()) || request.IdCompany == Guid.Empty)
        {
            return BadRequest("The company ID is required.");
        }

        var token = _tokenService.GenerateToken(request.IdCompany.ToString());
        return Ok(new { token });
    }
}