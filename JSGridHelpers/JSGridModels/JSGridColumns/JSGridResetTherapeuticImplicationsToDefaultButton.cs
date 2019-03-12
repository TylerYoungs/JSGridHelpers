using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns
{
    public class JSGridResetTherapeuticImplicationsToDefaultButton : JSGridButtonColumn
    {
        public JSGridResetTherapeuticImplicationsToDefaultButton(string nameOfCPropertyToBindValueTo) : base(ColumnType.resetTherapeuticImplicationsToDefaultButton, "Reset to Default", nameOfCPropertyToBindValueTo) { }
    }
}