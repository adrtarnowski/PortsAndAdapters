using Kitbag.Builder.Core.Domain;

namespace TLJ.PortsAndAdapters.Core.Domain.Rules
{
    public class DuplicateValueException : IBusinessRule
    {
        public bool IsValid() => true;
        public string BrokenRuleMessage => $"Entity already exists";
        public string Code => "duplicate-rule-broken";
    }
}