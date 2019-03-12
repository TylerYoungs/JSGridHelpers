using JSGridModels.JSGridColumns.Enums;
using JSGridModels.JSGridColumns.Interfaces;

namespace JSGridModels.JSGridColumns.Abstracts
{
    public abstract class JSGridTitledAndNamedColumn : JSGridColumn, IJSGridNamedColumn, IJSGridTitledColumn
    {
        private string _title;

        public JSGridTitledAndNamedColumn(ColumnType columnType, string title, string name) : base(columnType)
        {
            this.name = name;
            this.title = title;
        }

        public string name { get; }//the field that the javascript looks for in the C# object to bind data to
        public string title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_title))
                {
                    return System.Text.RegularExpressions.Regex.Replace(name, "(?!^)([A-Z])", " $1");
                }

                return _title;
            }
            private set
            {
                _title = value;
            }
        }
    }
}