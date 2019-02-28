using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
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