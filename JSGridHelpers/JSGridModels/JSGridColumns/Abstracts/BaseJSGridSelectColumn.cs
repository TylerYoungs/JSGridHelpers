using JSGridModels.JSGridColumns.Enums;
using System.Collections.Generic;

namespace JSGridModels.JSGridColumns.Abstracts
{
    public abstract class BaseJSGridSelectColumn<SelectOptionsType> : JSGridTitledAndNamedColumn
    {
        public BaseJSGridSelectColumn(List<SelectOptionsType> items, string textField, string valueField, string propertyToBindTo, bool allowEditing, ColumnType columnType, string title = null) : base(columnType, title, propertyToBindTo)
        {
            this.items = items;
            this.textField = textField;
            this.valueField = valueField;
            editing = allowEditing;
        }

        public override bool sorting => false;

        public List<SelectOptionsType> items { get; set; }
        public string textField { get; set; }
        public string valueField { get; set; }
    }
}