using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;
using JSGridModels.JSGridColumns.Interfaces;

namespace JSGridModels.JSGridColumns
{
    public class JSGridInformationColumn : JSGridColumn, IJSGridNamedColumn
    {
        private string _name;

        public JSGridInformationColumn(string name) : base(ColumnType.hidden)
        {
            editing = true;
            _name = name;
        }

        public string css => "hidden";
        public string name => _name;
    }
}
