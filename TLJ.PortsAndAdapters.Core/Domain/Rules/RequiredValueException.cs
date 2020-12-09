using Kitbag.Builder.Core.Domain;

namespace TLJ.PortsAndAdapters.Core.Domain.Rules
{
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
}