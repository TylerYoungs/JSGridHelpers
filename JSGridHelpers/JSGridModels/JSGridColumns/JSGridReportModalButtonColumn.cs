using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns
{
    public class JSGridReportModalButtonColumn : JSGridButtonColumn
    {
        public JSGridReportModalButtonColumn(string nameOfCPropertyToBindValueTo) : base(ColumnType.reportModalButton, "Report", nameOfCPropertyToBindValueTo) { }
    }
}