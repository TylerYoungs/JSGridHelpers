using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;
using System.Collections.Generic;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridMultiSelectColumn<SelectOptionsType> : BaseJSGridSelectColumn<SelectOptionsType>
    {
        public JSGridMultiSelectColumn(List<SelectOptionsType> items, string textField, string valueField, string propertyToBindTo, bool allowEditing, string title = null) : base(items, textField, valueField, propertyToBindTo, allowEditing, ColumnType.multiSelect, title) { }
    }
}