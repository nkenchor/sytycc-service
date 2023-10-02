using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceDomain = Sytycc_Service.Domain;
using Sytycc_Service.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Sytycc_Service.Api;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var result = await _userService.CreateUser(createUserDto);
          
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [Authorize]
    [HttpPut("{reference}")]
    public async Task<IActionResult> UpdateUser(string reference, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var result = await _userService.UpdateUser(reference, updateUserDto);
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [Authorize]
    [HttpDelete("{reference}")]
    public async Task<IActionResult> DeleteUser(string reference)
    {
        try
        {
            await GetUserByReference(reference);
            var result = await _userService.DeleteUser(reference);
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [Authorize]
    [HttpGet("{reference}")]
    public async Task<IActionResult> GetUserByReference(string reference)
    {
        try
        {
            var result = await _userService.GetUserByReference(reference) ?? throw new NotFoundException($"No user found with reference: {reference}");
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    
    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUserName(string username)
    {
        try
        {
            var result = await _userService.GetUserByUserName(username) ?? throw new NotFoundException($"No user found with username: {username}");
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [Authorize]
    [HttpGet("list/{page:int:min(1)}")]
    public async Task<IActionResult> GetUserList(int page)
    {
        try
        {
            var result = await _userService.GetUserList(page);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No users found for page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [Authorize]
    [HttpPost("search/{page:int:min(1)}")]
    public async Task<IActionResult> SearchUserList(int page,[FromQuery] string fullname)
    {
        try
        {
            var result = await _userService.SearchUserList(page, fullname);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No users found with name: {fullname} on page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto userDto)
    {
        try
        {
            var result = await _userService.Login(userDto);

            if (string.IsNullOrEmpty(result))
                throw new ServiceDomain.UnauthorizedAccessException("Invalid credentials."); //
            
            var cookieOptions = new CookieOptions
            {
    
                HttpOnly = true,
                Secure = true,  
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt16(Service.TokenExpiry)),
            };
            Response.Cookies.Append("jwtToken", result, cookieOptions);

            return Ok(new { token = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [HttpPost("logout")]

    public IActionResult Logout()
    {
        // Clear the JWT cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,  
            Expires = DateTime.UtcNow.AddDays(-1)  
        };

        Response.Cookies.Append("jwtToken", "", cookieOptions);

        return Ok(new { message = "Logged out successfully" });
    }



}

