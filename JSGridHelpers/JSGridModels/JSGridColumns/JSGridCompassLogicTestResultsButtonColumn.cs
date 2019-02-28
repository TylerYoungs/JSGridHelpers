using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridCompassLogicTestResultsButtonColumn : JSGridButtonColumn
    {
        public JSGridCompassLogicTestResultsButtonColumn(string nameOfCPropertyToBindValueTo) : base(ColumnType.compassLogicTestResultsButton, "Test Results", nameOfCPropertyToBindValueTo) { }
    }
}


