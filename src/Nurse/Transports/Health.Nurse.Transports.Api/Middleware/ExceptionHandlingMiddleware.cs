using Health.Shared.Application.Serialization;
using Health.Shared.Workflow.Processes.Exceptions;

namespace Health.Nurse.Transports.Api.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly JsonSerializer _serializer;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, JsonSerializer serializer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new NurseApiGenericValidationResultObject(GetTitle(exception), statusCode, exception.Message, GetErrors(exception));
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(_serializer.Serialize(response));
    }
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            WorkflowValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    private static string GetTitle(Exception exception) =>
        exception switch
        {
            WorkflowValidationException valException => "Validation Failure",
            ApplicationException applicationException => applicationException.Message,
            _ => "Server Error"
        };
    private IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
    {
        //IReadOnlyDictionary<string, string[]> errors = null;
        var errors = new Dictionary<string, string[]>();
        if (exception is WorkflowValidationException domainValidationException)
        {
            if (domainValidationException.Errors != null)
                errors = domainValidationException.Errors.GroupBy(
                        x => x.PropertyName,
                        x => x.ErrorMessage,
                        (propertyName, errorMessages) => new
                        {
                            Key = propertyName ?? "Unknown",
                            Values = errorMessages.Distinct().ToArray()
                        })
                    .ToDictionary(x => x.Key, x => x.Values);
        }
        else
        {
            _logger.LogError("Unknown Server Error", exception);
            errors = new Dictionary<string, string[]>()
            {
                {"Server_Error", new string[]{"Unknown server error occured. Please contact administrator is problem persists"} }
            };
        }
        
        return errors;
    }
}