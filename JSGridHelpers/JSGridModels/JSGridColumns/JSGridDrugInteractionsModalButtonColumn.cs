using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridDrugGeneInteractionsModalButtonColumn : JSGridButtonColumn
    {
        public JSGridDrugGeneInteractionsModalButtonColumn(string nameOfCPropertyToBindValueTo) : base(ColumnType.drugInteractionsModalButton, "Drug-Gene Interactions", nameOfCPropertyToBindValueTo) { }
    }
}


