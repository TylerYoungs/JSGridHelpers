using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridReportModalButtonColumn : JSGridButtonColumn
    {
        public JSGridReportModalButtonColumn(string nameOfCPropertyToBindValueTo) : base(ColumnType.reportModalButton, "Report", nameOfCPropertyToBindValueTo) { }
    }
}