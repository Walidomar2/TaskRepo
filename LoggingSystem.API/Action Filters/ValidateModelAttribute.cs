using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoggingSystem.API.Action_Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly bool _includeErrorDetails;
        public ValidateModelAttribute(bool includeErrorDetails = true)
        {
            _includeErrorDetails = includeErrorDetails;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (_includeErrorDetails)
                {
                    var errors = context.ModelState
                        .Where(x => x.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    context.Result = new BadRequestObjectResult(new
                    {
                        StatusCode = 400,
                        Message = "Validation failed.",
                        Errors = errors
                    });
                }
                else
                {
                    context.Result = new BadRequestResult();
                }
            }
        }
    }
}
