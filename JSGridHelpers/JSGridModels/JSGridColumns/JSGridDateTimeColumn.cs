using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns
{
    public class JSGridDateTimeColumn : JSGridTitledAndNamedColumn
    {
        public JSGridDateTimeColumn(string name, bool allowEditing, string title = null) : base(ColumnType.dateTime, title, name)
        {
            editing = allowEditing;
        }
    }
}