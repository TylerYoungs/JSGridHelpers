using System.Collections.Generic;
using System.Linq;
using JSGridModels.Extensions;

namespace JSGridModels.Helpers
{
    public static class SortingHelper    {
        public static IEnumerable<TypeToSort> GetSortedItems<TypeToSort, T>(IEnumerable<TypeToSort> data, T param) where T : IColumnSort
        {
            if (param.ColumnSort?.IsSortedByAscending == true && param.ColumnSort.StringAndOrderContainValues)
            {
                data = data.OrderBy(param.ColumnSort.Name).ToList();
            }
            else if (param.ColumnSort?.IsSortedByAscending == false && param.ColumnSort.StringAndOrderContainValues)
            {
                data = data.OrderByDescending(param.ColumnSort.Name).ToList();
            }

            return data;
        }
    }

    public interface IColumnSort
    {
        ColumnSort ColumnSort { get; set; }
    }

    public class ColumnSort
    {
        public string Name { get; set; }
        public string Order { get; set; }
        public bool StringAndOrderContainValues => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Order);
        public bool? IsSortedByAscending => Order?.Equals("asc");
    }
}
