using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridDateTimeColumn : JSGridTitledAndNamedColumn
    {
        public JSGridDateTimeColumn(string name, bool allowEditing, string title = null) : base(ColumnType.dateTime, title, name)
        {
            editing = allowEditing;
        }
    }
}