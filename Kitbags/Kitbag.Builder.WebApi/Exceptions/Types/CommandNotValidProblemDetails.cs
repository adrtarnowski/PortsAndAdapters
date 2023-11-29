using System.Collections.Generic;
using System.Linq;
using Kitbag.Builder.WebApi.Exceptions.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kitbag.Builder.WebApi.Exceptions.Types
{
    public class CommandNotValidProblemDetails : ProblemDetails
    {
        public List<ValidationErrorDto> ValidationErrors { get; }

        public CommandNotValidProblemDetails(CommandNotValidException exception)
        {
            var notFoundProblem = exception
                .ValidationFailures
                .FirstOrDefault(vf => vf.ErrorCode == ValidationErrorCode.NotFound);

            Status = notFoundProblem != null ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;
            Type = notFoundProblem != null ? ValidationErrorCode.NotFound : ValidationErrorCode.CommandNotValid;
            Title = "Request parameters didn't validate.";
            ValidationErrors = notFoundProblem != null ?
                new List<ValidationErrorDto> { new ValidationErrorDto(notFoundProblem)}
                : exception.ValidationFailures
                    .Select(vf => new ValidationErrorDto(vf))
                    .ToList();
        }
    }
}