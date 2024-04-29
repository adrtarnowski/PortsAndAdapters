using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kitbag.Builder.WebApi.Exceptions.Types;

public class ConcurrencyProblemDetails : ProblemDetails
{
    public ConcurrencyProblemDetails()
    {
        Title = "In the meantime the values have been changed. Please, check the current values and try again.";
        Status = StatusCodes.Status400BadRequest;
        Type = "concurrency-exception";
    }
}