using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ServerApi.Authentication;

using ServerCore.Authentications.Commands;
using ServerCore.Authentications.Queries;

namespace ServerApi.Controllers;

public class AuthenticationController : ApiControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly CookieService _cookieService;

    public AuthenticationController(IConfiguration configuration, CookieService cookieService) : base()
    {
        _configuration = configuration;
        _cookieService = cookieService;
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUpCustomer([FromBody] SignUpCustomerCommand command)
    {
        await Mediator.Send(command);
        return await LogIn(new LogInQuery { Email = command.Email, Password = command.Password });
    }

    [HttpGet("LogIn")]
    public async Task<IActionResult> LogIn([FromQuery] LogInQuery query)
    {
        var response = await Mediator.Send(query);

        await _cookieService.SetCookie(response.UserId, HttpContext);

        return Ok();
    }

    [HttpPost("LogOut")]
    public async Task<IActionResult> LogOut()
    {
        await _cookieService.RemoveCookie(HttpContext);
        return Ok();
    }

    [HttpPost("UpdatePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommand command)
    {
        await Mediator.Send(command);
        return await LogOut();
    }

    [HttpPost("RequestForgotPassword")]
    public async Task<IActionResult> RequestForgotPassword([FromBody] RequestForgotPasswordCommand command)
    {
        command.ExpirationMinutes = _configuration.GetValue<int>("Authentication:ForgotPasswordRequestExpirationMinutes");
        await Mediator.Send(command);
        return await LogOut();
    }

    [HttpPost("UpdateForgotPassword")]
    public async Task<IActionResult> UpdateForgotPassword([FromBody] UpdateForgotPasswordCommand command)
    {
        await Mediator.Send(command);
        return await LogOut();
    }

    [HttpPost("DeleteAccount")]
    public async Task<IActionResult> DeleteAccount([FromBody] UpdateForgotPasswordCommand command)
    {
        await Mediator.Send(command);
        return await LogOut();
    }
}