using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.Auth;
using MySocial.WebApi.Security;

namespace MySocial.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService, IAuthService authService)
        {
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost]
        [Route("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loginResponse = await _authService.ValidateLogin(request);

            if (!loginResponse.IsAuthenticated)
                return BadRequest("Usuário e/ou senha inválidos");

            var token = _tokenService.GenerateToken(loginResponse);

            return Ok(new
            {
                user = loginResponse,
                token
            });
        }
    }
}
