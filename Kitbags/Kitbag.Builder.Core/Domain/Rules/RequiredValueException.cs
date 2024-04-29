namespace Kitbag.Builder.Core.Domain.Rules;

public class RequiredValueException : IBusinessRule
{
    public string FieldName { get; }
    public RequiredValueException(string fieldName)
    {
        FieldName = fieldName;
    }
    public bool IsValid() => true;
    public string BrokenRuleMessage => $"Field {FieldName} is required";
    public string Code => "required-rule-broken";
}