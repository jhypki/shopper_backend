using ShopperBackend.Models;
using ShopperBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ShopperBackend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;
    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var users = await _usersService.GetAsync();
        return Ok(users);
    }

}