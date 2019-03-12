﻿using JSGridModels.JSGridColumns.Abstracts;
using JSGridModels.JSGridColumns.Enums;

namespace JSGridModels.JSGridColumns
{
    public class JSGridNumberColumn : JSGridTitledAndNamedColumn
    {
        public JSGridNumberColumn(string name, bool allowEditing, string title = null) : base(ColumnType.text, title, name)
        {
            editing = allowEditing;
        }
    }
}