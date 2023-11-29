using System;
using System.Collections.Generic;
using System.Linq;
using Kitbag.Builder.CQRS.Core.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Kitbag.Builder.WebApi.Exceptions.Handlers
{
    public class EmptyCommandFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid
               && HasActionCommandAsParameter(context))
            {
                var problemDetails = new ProblemDetails();
                problemDetails.Instance = context.HttpContext.Request.Path.ToString();
                problemDetails.Title = "Input (json) command has wrong format";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Type = "invalid-json-input-command";
                problemDetails.Detail = string.Join(Environment.NewLine, GetErrorsFromModelState(context.ModelState));

                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }

        private bool HasActionCommandAsParameter(ActionExecutingContext context)
        {
            return context.ActionDescriptor.Parameters.Any(x => x.ParameterType.GetInterfaces().Any(y => y.Name == nameof(ICommand)));
        }

        private IEnumerable<string> GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            foreach (var modelStateDictionary in modelState.Values)
            {
                foreach (ModelError error in modelStateDictionary.Errors)
                {
                    yield return error.ErrorMessage;
                }
            }
        }
    }
}