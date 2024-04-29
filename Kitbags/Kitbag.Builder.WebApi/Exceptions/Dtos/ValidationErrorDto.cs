using FluentValidation.Results;

namespace Kitbag.Builder.WebApi.Exceptions.Dtos;

public class ValidationErrorDto
{
    public string PropertyName { get; }
    public string ErrorMessage { get; }
    public ValidationErrorTypes? ErrorType { get; }

    public ValidationErrorDto(ValidationFailure validationFailure)
    {
        PropertyName = validationFailure.PropertyName;
        ErrorMessage = validationFailure.ErrorMessage;
        ErrorType = ValidationErrorTypeMapper.Fill(validationFailure);
    }
}