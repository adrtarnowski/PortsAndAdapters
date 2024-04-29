namespace Kitbag.Builder.Core.Domain;

public interface IBusinessRule
{
    bool IsValid();
    string? BrokenRuleMessage { get; }
    string Code { get; }
}