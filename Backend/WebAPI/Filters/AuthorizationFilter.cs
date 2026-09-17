using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModelsAPI;
using ServicesInterfaces.Sessions;

namespace WebAPI.Filters;

public class AuthorizationFilter : Attribute, IAuthorizationFilter
{
    private readonly List<string> _role;
    private const string AUTHORIZATION_HEADER = "Authorization";

    public AuthorizationFilter(string role)
    {
        _role = new List<string> { role };
    }
    public AuthorizationFilter(params string[] roles)
    {
        _role = roles.ToList();
    }
   

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers[AUTHORIZATION_HEADER];

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Unauthenticated",
                Message = "You are not authenticated"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        try
        {
            var logic = GetSessionLogic(context);
            if (!logic.ValidSession(authorizationHeader, _role))
            {
                GenericResponse response = new GenericResponse
                {
                    Message = "Unauthorized",
                    ExecutionSuccessful = false
                };
                context.Result = new ObjectResult(response)
                {
                    StatusCode = 403,
                };
            }
            else
            {
                context.HttpContext.Items.Add("user", logic.GetUserFromSession(authorizationHeader));
            }
        }
        catch (Exception ex)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "InternalError",
                Message = "An error ocurred while processing the request"
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    private ISessionService GetSessionLogic(AuthorizationFilterContext context)
    {
        var sessionType = typeof(ISessionService);
        return context.HttpContext.RequestServices.GetService(sessionType) as ISessionService;
    }
}