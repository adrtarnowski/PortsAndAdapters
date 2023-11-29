using System;
using FluentValidation.Results;

namespace Kitbag.Builder.WebApi.Exceptions.Types
{
    public class QueryNotValidException : Exception
    {
        public ValidationFailure[] ValidationFailures { get; }

        public QueryNotValidException(params ValidationFailure[] validationFailures)
        {
            ValidationFailures = validationFailures;
        }
    }
}