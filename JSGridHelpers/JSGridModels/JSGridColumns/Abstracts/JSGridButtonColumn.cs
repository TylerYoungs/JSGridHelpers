using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns.Abstracts
{
    public abstract class JSGridButtonColumn : JSGridTitledAndNamedColumn
    {
        private ColumnType _columnType;

        public JSGridButtonColumn(ColumnType columnType, string title, string name) : base(columnType, title, name)
        {
            _columnType = columnType;
            editing = false;
            readOnly = true;
        }

        public override bool sorting => false;

        public override string type
        {
            get
            {
                var buttonType = _columnType.ToString();
                if (!buttonType.ToLower().Contains("button"))
                {
                    return buttonType + "Button";
                }

                return buttonType;
            }
        }
    }
}