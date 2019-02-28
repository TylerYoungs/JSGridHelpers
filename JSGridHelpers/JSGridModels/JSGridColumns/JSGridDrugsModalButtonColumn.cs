using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridDrugsModalButtonColumn : JSGridButtonColumn
    {
        public JSGridDrugsModalButtonColumn(string nameOfCPropertyToBindValueTo) : base(ColumnType.drugsModalButton, "Drugs", nameOfCPropertyToBindValueTo) { }
    }
}