using System;
using FluentValidation.Results;

namespace Kitbag.Builder.WebApi.Exceptions.Types;

public class CommandNotValidException : Exception
{
    public ValidationFailure[] ValidationFailures { get; }

    public CommandNotValidException(params ValidationFailure[] validationFailures)
    {
        ValidationFailures = validationFailures;
    }
}