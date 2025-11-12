
namespace DemoMinimalAPI.Filters;

public class NotEmptyFilter : IEndpointFilter
{
    public ValueTask<object?> 
        InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var x = context.Arguments.FirstOrDefault
            (arg => arg is string) is string s1;


       var check =  context.Arguments.FirstOrDefault
            (arg => arg is string) is string s &&
            string.IsNullOrWhiteSpace(s);
       if (check is false)
       {
            return ValueTask.FromResult<object?>(Results.BadRequest("Empty Value."));
       }

       return next(context);
    }
}
