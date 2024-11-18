using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;
using PrasenssaAPI.Models;
using PrasenssaAPI.PrasenssaConnection;

namespace PrasenssaAPI.Controllers;

[ApiController]
[Route("/login")]
[OpenApiRule]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;

    private IPraseansaClient _praseansaClient;
    
    public LoginController(ILogger<LoginController> logger, IPraseansaClient praseansaClient)
    {
        _logger = logger;
        _praseansaClient = praseansaClient;
    }
    
    [HttpPost]
    public LoginResponse PostLogin([FromBody] LoginForm loginForm)
    {
        Guid guid = Guid.NewGuid();
        _praseansaClient.SetConnectionParams(guid.ToString(),new ConnectionParams(loginForm.User, loginForm.Pwd,loginForm.Ip));
        HttpContext.Response.Cookies.Append("SessionID", guid.ToString());
        return new LoginResponse(_praseansaClient.Connect( loginForm.User, loginForm.Pwd, loginForm.Ip).ToString());
    }
    
}

public class LoginResponse
{
    public string response { get; set; }

    public LoginResponse(string response)
    {
        this.response = response;
    }
}