using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModelException;
using ModelsAPI;

namespace WebAPI.Filters;

public class ExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        int statusCode = 500;
        GenericResponse response = new GenericResponse
        {
            ExecutionSuccessful = false,
            Message = "An unexpected internal server error, please try again later or contact support."
        };
        
        switch (context.Exception)
        {
            case BadRequestException badRequestException:
                statusCode = 400;
                response.Message = $"{badRequestException.Message}";
                break;
            
            case UnauthorizedException:
                statusCode = 401;
                response.Message = "You do not have permission to perform this action.";
                break;
            
            case ForbiddenException:
                statusCode = 403;
                response.Message = "You do not have permission to perform this action.";
                break;
            
            case NotFoundException notFoundException:
                statusCode = 404;
                response.Message = $"{notFoundException.Message}.";
                break;

            case ConflictException conflictException:
                statusCode = 409;
                response.Message = $"{conflictException.Message}";
                break;
            
            case Exception:
                statusCode = 500;
                response.Message = "An unexpected internal server error, please try again later or contact support.";
                break;
        }
        
        context.Result = new ObjectResult(response)
        {
            StatusCode = statusCode
        };
    }
}