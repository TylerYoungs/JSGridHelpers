using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns
{
    public class JSGridCheckboxColumn : JSGridTitledAndNamedColumn
    {
        public JSGridCheckboxColumn(string name, bool allowEditing, string title = null) : base(ColumnType.checkbox, title, name)
        {
            sorting = true;
            editing = allowEditing;
        }
    }
}