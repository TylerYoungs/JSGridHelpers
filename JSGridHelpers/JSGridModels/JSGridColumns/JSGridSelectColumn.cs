using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;
using System.Collections.Generic;

namespace JSGridModels.JSGridColumns
{
    public class JSGridSelectColumn<SelectOptionsType> : BaseJSGridSelectColumn<SelectOptionsType>
    {
        public JSGridSelectColumn(List<SelectOptionsType> items, string textField, string valueField, string propertyToBindTo, bool allowEditing, string title = null) : base(items, textField, valueField, propertyToBindTo, allowEditing, ColumnType.select, title) { }
    }
}