using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns
{
    public class JSGridDrugsModalButtonColumn : JSGridButtonColumn
    {
        public JSGridDrugsModalButtonColumn(string nameOfCPropertyToBindValueTo) : base(ColumnType.drugsModalButton, "Drugs", nameOfCPropertyToBindValueTo) { }
    }
}