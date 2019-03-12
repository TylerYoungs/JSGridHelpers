using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;
using JSGridModels.JSGridColumns.Interfaces;

namespace JSGridModels.JSGridColumns
{
    public class JSGridHiddenInformationColumn : JSGridColumn, IJSGridNamedColumn, IJSGridUneditableColumn
    {
        public JSGridHiddenInformationColumn(string name) : base(ColumnType.hidden)
        {
            this.name = name;
            editing = true;
        }

        public string css => ColumnType.hidden.ToString();
        public string name { get; }
    }
}