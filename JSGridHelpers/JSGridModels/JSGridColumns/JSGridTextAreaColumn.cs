using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
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