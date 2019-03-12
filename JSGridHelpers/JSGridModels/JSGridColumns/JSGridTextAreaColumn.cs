using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns
{
    public class JSGridTextAreaColumn : JSGridTitledAndNamedColumn
    {
        public JSGridTextAreaColumn(string name, bool allowEditing, string title = null) : base(ColumnType.textarea, title, name)
        {
            editing = allowEditing;
            sorting = false;
        }
    }
}