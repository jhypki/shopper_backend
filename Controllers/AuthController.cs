using Microsoft.AspNetCore.Mvc;
using ShopperBackend.Models;
using ShopperBackend.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ShopperBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(AuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {

        if (string.IsNullOrEmpty(newUser.PasswordHash))
        {
            return BadRequest("Password cannot be null or empty.");
        }

        var existingUser = await _authService.GetByUsernameAsync(newUser.Username);

        if (existingUser != null)
        {
            return BadRequest("Username already exists.");
        }

        newUser.PasswordHash = AuthService.HashPassword(newUser.PasswordHash); // Hash the password
        await _authService.CreateAsync(newUser);
        var token = GenerateJwtToken(newUser);
        return Ok(new { Token = token });
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _authService.GetByUsernameAsync(model.Username);

        if (user == null || user.PasswordHash != AuthService.HashPassword(model.Password))
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            user.Id != null ? new Claim(ClaimTypes.NameIdentifier, user.Id) : throw new ArgumentNullException(nameof(user.Id)),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginModel
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
