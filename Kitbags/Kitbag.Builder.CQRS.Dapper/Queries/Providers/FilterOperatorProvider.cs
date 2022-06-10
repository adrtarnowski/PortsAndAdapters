using Kitbag.Builder.CQRS.Core.Queries.DTO;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Providers
{
    public static class FilterOperatorProvider
    {
        public static string? GetSqlComparisonOperator(this FilteringConfiguration.Filter filter)
        {
            switch (filter.Operator)
            {
                case FilteringConfiguration.FilterComparisonOperator.DateEquals:
                case FilteringConfiguration.FilterComparisonOperator.Equals:
                    return "=";
                case FilteringConfiguration.FilterComparisonOperator.Contains:
                    return "LIKE";
                case FilteringConfiguration.FilterComparisonOperator.GreaterThan:
                    return ">";
                case FilteringConfiguration.FilterComparisonOperator.LowerThan:
                    return "<";
                case FilteringConfiguration.FilterComparisonOperator.In:
                    return "IN";
                case FilteringConfiguration.FilterComparisonOperator.IsNull:
                    return "IS NULL";
                case FilteringConfiguration.FilterComparisonOperator.IsNotNull:
                    return "IS NOT NULL";
                case FilteringConfiguration.FilterComparisonOperator.Between:
                    return "BETWEEN";
            }

            return null;
        }
    }
}