namespace Kitbag.Builder.WebApi.Exceptions.Dtos
{
    public static class ValidationErrorCode
    {
        public const string NotFound = "not-found";
        public const string CommandNotValid = "validation-error";
        public const string QueryNotValid = "query-not-valid";
    }

    public enum ValidationErrorTypes
    {
        Required
    }

    public enum ValidationFailureErrorCodes
    {
        NotEmptyValidator
    }
}