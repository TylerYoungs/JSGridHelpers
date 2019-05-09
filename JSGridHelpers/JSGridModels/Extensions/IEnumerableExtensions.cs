using System.Collections.Generic;
using System.Linq;
using JSGridModels.Exceptions;
using JSGridModels.JSGridColumns;
using JSGridModels.JSGridColumns.Abstracts;

namespace JSGridModels.Extensions
{
    public static class IEnumerableExtensions
    {
        public static JSGridTable GetJSGridTableFromIEnumerableOfData<ReturnType>(this IEnumerable<ReturnType> records, string name, bool allowEditing) where ReturnType : class
        {
            var jsGridTable = new JSGridTable();
            jsGridTable.fields = new List<JSGridColumn>();

            jsGridTable.Name = name;

            var firstRecord = records?.FirstOrDefault();
            var props = firstRecord?.GetType().GetProperties();

            foreach (var prop in props)
            {
                var type = prop.PropertyType;
                JSGridColumn jsGridColumn;

                if (type.Equals(typeof(int)) || type.Equals(typeof(int?)) || type.Equals(typeof(decimal)) || type.Equals(typeof(decimal?)) || type.Equals(typeof(double)) || type.Equals(typeof(double?)))
                {
                    jsGridColumn = new JSGridNumberColumn(prop.Name, allowEditing);
                }
                else if (type.Equals(typeof(string)) || type.Equals(typeof(char)))
                {
                    jsGridColumn = new JSGridTextColumn(prop.Name, allowEditing);
                }
                else if (type.Equals(typeof(bool)) || type.Equals(typeof(bool?)))
                {
                    jsGridColumn = new JSGridCheckboxColumn(prop.Name, allowEditing);
                }
                else
                {
                    throw new ColumnNotAcceptableException($"Property named {prop.Name} not availble for conversion to JSGridColumn type.");
                }

                jsGridTable.fields.Add(jsGridColumn);
            }

            jsGridTable.data = JSGridTable.GetDataAsObjects(records);

            return jsGridTable;
        }
    }
}
