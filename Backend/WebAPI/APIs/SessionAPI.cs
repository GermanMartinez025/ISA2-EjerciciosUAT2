using ControllersInterfaces.Sessions;
using Microsoft.AspNetCore.Mvc;
using ModelsAPI.Sessions;
using WebAPI.Filters;

namespace WebAPI.APIs;

[ApiController]
[Route("api/sessions")]
[ExceptionFilter]
public class SessionAPI : ControllerBase
{
    private readonly ISessionController _sessionController;
    public SessionAPI(ISessionController sessionController)
    {
        _sessionController = sessionController;
    }
    
    [HttpPost]
    public ResponseCreateSession CreateSession([FromBody] RequestCreateSession request)
    {
        return  _sessionController.CreateSession(request);
    }
    
    [HttpDelete]
    public IActionResult DeleteSession()
    {
        return Ok();
    }
    
}