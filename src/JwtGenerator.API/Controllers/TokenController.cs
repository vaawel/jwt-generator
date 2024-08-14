using Asp.Versioning;
using JwtGenerator.API.Contract.Requests;
using JwtGenerator.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtGenerator.API.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/token")]
public class TokenController(TokenService tokenService, ILogger<TokenController> logger) : ControllerBase
{
    private readonly TokenService _tokenService = tokenService;
    private readonly ILogger<TokenController> _logger = logger;

    [HttpPost("generate")]
    public IActionResult GenerateToken([FromBody] GenerateTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.IdCompany.ToString()) || request.IdCompany == Guid.Empty)
        {
            _logger.LogError("JWT wasn't generated. Need the company ID to do so.");
            return BadRequest("The company ID is required.");
        }

        var token = _tokenService.GenerateToken(request.IdCompany.ToString());
        _logger.LogInformation($"JWT for company {request.IdCompany} generated.");
        return Ok(new { token });
    }
}