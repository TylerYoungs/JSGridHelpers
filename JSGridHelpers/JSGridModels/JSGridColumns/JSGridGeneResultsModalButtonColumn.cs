using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridGeneResultsModalButtonColumn: JSGridButtonColumn
    {
        public JSGridGeneResultsModalButtonColumn(string nameOfCPropertyToBindValueTo) : base(ColumnType.geneResultsModalButton, "Gene Results", nameOfCPropertyToBindValueTo) { }
    }
}