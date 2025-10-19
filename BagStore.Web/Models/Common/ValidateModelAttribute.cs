using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BagStore.Models.Common;
using BagStore.Web.Models.Common;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .SelectMany(kvp => kvp.Value.Errors.Select(err =>
                    new ErrorDetail(kvp.Key, err.ErrorMessage)))
                .ToList();

            // Trả về chuẩn BaseResponse
            context.Result = new BadRequestObjectResult(BaseResponse<object>.Error(errors));
        }
    }
}