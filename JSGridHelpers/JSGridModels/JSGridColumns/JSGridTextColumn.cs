﻿using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using BioTapMedical.Models.JSGridModels.JSGridColumns.Enums;

namespace BioTapMedical.Models.JSGridModels.JSGridColumns
{
    public class JSGridTextColumn : JSGridTitledAndNamedColumn
    {
        public JSGridTextColumn(string name, bool allowEditing, string title = null) : base(ColumnType.text, title, name)
        {
            editing = allowEditing;
        }
    }
}