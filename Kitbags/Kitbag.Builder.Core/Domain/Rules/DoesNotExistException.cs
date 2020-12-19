namespace Kitbag.Builder.Core.Domain.Rules
{
    public class DoesNotExistException : IBusinessRule
    {
        public bool IsValid() => true;
        public string BrokenRuleMessage => "Entity doesn't exist";
        public string Code => "entity-does-not-exist";
    }
}