using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoDealerPro.Shared.Abstractions.Filter;

public class ValidateRequestFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.TryGetValue("request", out var request) && request != null)
        {
            var requestType = request.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(requestType);

            if (context.HttpContext.RequestServices.GetService(validatorType) is IValidator validator)
            {
                var result = await validator.ValidateAsync(new ValidationContext<object>(request));

                if (!result.IsValid)
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    });
                    return;
                }
            }
        }

        await next();
    }
}
