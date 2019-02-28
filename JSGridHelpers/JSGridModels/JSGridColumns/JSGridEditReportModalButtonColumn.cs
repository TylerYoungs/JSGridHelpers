using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridEditReportModalButtonColumn : JSGridButtonColumn
    {
        public JSGridEditReportModalButtonColumn(string name = null) : base(ColumnType.editReportModalButton, "Edit Report", name) { }
    }
}