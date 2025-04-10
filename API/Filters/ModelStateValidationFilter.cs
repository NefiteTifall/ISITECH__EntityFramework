using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ModelStateValidationFilter : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		if (!context.ModelState.IsValid)
		{
			var errors = context.ModelState
				.Where(e => e.Value.Errors.Count > 0)
				.ToDictionary(
					kvp => kvp.Key,
					kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
				);

			var problemDetails = new ValidationProblemDetails(context.ModelState)
			{
				Status = StatusCodes.Status400BadRequest
			};

			context.Result = new BadRequestObjectResult(problemDetails);
		}
	}
}