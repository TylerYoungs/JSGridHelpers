using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridResetTherapeuticImplicationsToDefaultButton : JSGridButtonColumn
    {
        public JSGridResetTherapeuticImplicationsToDefaultButton(string nameOfCPropertyToBindValueTo) : base(ColumnType.resetTherapeuticImplicationsToDefaultButton, "Reset to Default", nameOfCPropertyToBindValueTo) { }
    }
}