using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns
{
    public class JSGridGeneResultsModalButtonColumn: JSGridButtonColumn
    {
        public JSGridGeneResultsModalButtonColumn(string nameOfCPropertyToBindValueTo) : base(ColumnType.geneResultsModalButton, "Gene Results", nameOfCPropertyToBindValueTo) { }
    }
}