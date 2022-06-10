using System;
using Kitbag.Builder.CQRS.Core.Queries.DTO;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Providers
{
    public static class SortDirectionProvider
    {
        public static string GetSortDirection(this SortingConfiguration direction)
        {
            switch (direction.Order)
            {
                case SortingConfiguration.SortOrder.Asc:
                    return "ASC";
                case SortingConfiguration.SortOrder.Desc:
                    return "DESC";
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction.Order), direction.Order, null);
            }
        }
    }
}