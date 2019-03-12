using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns.Abstracts
{
    public abstract class JSGridColumn
    {
        public JSGridColumn(ColumnType columnType)
        {
            type = columnType.ToString();
            sorting = true;
            editing = false;
            readOnly = false;
        }

        public virtual int? Width { get; set; }
        public virtual bool readOnly { get; set; }
        public virtual bool sorting { get; set; }
        public virtual string type { get; }
        public virtual string width => Width == null ? null : Width.ToString() + "px";
        public virtual int? height { get; set; }
        public virtual bool editing { get; set; }
    }
}