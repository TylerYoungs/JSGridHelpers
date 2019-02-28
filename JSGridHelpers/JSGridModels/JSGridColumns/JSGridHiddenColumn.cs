using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Interfaces;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridHiddenInformationColumn : JSGridColumn, IJSGridNamedColumn, IJSGridUneditableColumn
    {
        public JSGridHiddenInformationColumn(string name) : base(ColumnType.hidden)
        {
            this.name = name;
            editing = true;
        }

        public string css => ColumnType.hidden.ToString();
        public string name { get; }
    }
}