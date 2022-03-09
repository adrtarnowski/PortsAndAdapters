using System.Collections.Generic;

namespace Kitbag.Builder.CQRS.Core.Queries.DTO
{
    public class FilteringConfiguration
    {
        public ICollection<Filter>? Filters { get; set; }
        public FilterLogicalOperator? Operator { get; set; }

        public class Filter
        {
            public string? Field { get; set; }
            public object? Value { get; set; }
            public FilterComparisonOperator? Operator { get; set; }
        }

        public enum FilterComparisonOperator
        {
            Equals,
            GreaterThan,
            LowerThan,
            Contains,
            In,
            IsNull,
            IsNotNull,
            DateEquals,
            Between
        }

        public enum FilterLogicalOperator
        {
            And,
            Or
        }
    }
}