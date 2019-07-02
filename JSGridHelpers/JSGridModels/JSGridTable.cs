using JSGridModels.Exceptions;
using JSGridModels.JSGridColumns;
using JSGridModels.JSGridColumns.Abstracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace JSGridModels
{
    //Any property that is lowercase will be consumed in the javascript JSGridHelper. If the property is uppercased, it is utilized purely in C#. This is a convention I'm establishing.
    public class JSGridTable
    {
        public JSGridTable()
        {
            editing = false;
            inserting = false;
            deleting = false;

            SetupDefaults();
        }

        public static JSGridTable GetJSGridTableFromIEnumerableOfData<ReturnType>(IEnumerable<ReturnType> records, string name, bool allowEditing) where ReturnType : class
        {
            var jsGridTable = new JSGridTable();
            jsGridTable.fields = new List<JSGridColumn>();

            jsGridTable.Name = name;

            if (records != null)
            {
                var typeOfReturn = typeof(ReturnType);

                if (typeOfReturn.Equals(typeof(ExpandoObject)))
                {
                    var exp = records.FirstOrDefault() as ExpandoObject;
                    if (exp != null)
                    {
                        for (int i = 0; i < exp.Count(); i++)
                        {
                            var kvp = exp.ElementAt(i);
                            jsGridTable.fields.Add(GetColumnFromType(false, kvp.Key, typeof(string)));
                        }
                    }
                }
                else
                {
                    var props = ((ReturnType)Activator.CreateInstance(typeOfReturn, new object[] { })).GetType().GetProperties();
                    jsGridTable.fields.AddRange(props.Select(prop => GetColumnFromType(allowEditing, prop.Name, prop.PropertyType)));
                }

                jsGridTable.data = GetDataAsObjects(records);
            }
            else
            {
                jsGridTable.data = new List<object>();
            }

            return jsGridTable;
        }

        private static JSGridColumn GetColumnFromType(bool allowEditing, string name, Type type)
        {
            JSGridColumn jsGridColumn;

            if (type.Equals(typeof(int)) || type.Equals(typeof(int?)) || type.Equals(typeof(decimal)) || type.Equals(typeof(decimal?)) || type.Equals(typeof(double)) || type.Equals(typeof(double?)) || type.Equals(typeof(long)) || type.Equals(typeof(long?)))
            {
                jsGridColumn = new JSGridNumberColumn(name, allowEditing);
            }
            else if (type.Equals(typeof(string)) || type.Equals(typeof(char)))
            {
                jsGridColumn = new JSGridTextColumn(name, allowEditing);
            }
            else if (type.Equals(typeof(bool)) || type.Equals(typeof(bool?)))
            {
                jsGridColumn = new JSGridCheckboxColumn(name, allowEditing);
            }
            else if (type.Equals(typeof(DateTime)) || type.Equals(typeof(DateTime?)))
            {
                jsGridColumn = new JSGridDateTimeColumn(name, allowEditing);
            }
            else
            {
                throw new ColumnNotAcceptableException($"Property named {name} not availble for conversion to JSGridColumn type.");
            }

            return jsGridColumn;
        }

        private void SetupDefaults()
        {
            WidthAsIntegerPercentage = 100;

            pageSize = 10;
            pageButtonCount = 5;

            filtering = true;
            sorting = true;
            paging = true;
            loadMessage = "Please, wait...";

            autoload = false;
            pageLoading = false;
        }

        public JSGridTable(List<object> rows, List<JSGridColumn> columns, bool allowEditingRows, bool allowDeletingRows, bool allowInsertingRows, string updateURL, string deleteURL, string insertURL)
        {
            data = rows;
            fields = columns;

            editing = allowEditingRows;
            deleting = allowDeletingRows;
            inserting = allowInsertingRows;

            this.updateURL = updateURL;
            this.deleteURL = deleteURL;
            this.insertURL = insertURL;

            SetupDefaults();
        }

        public int WidthAsIntegerPercentage { get; set; }
        public string width => WidthAsIntegerPercentage.ToString() + "%";

        public bool filtering { get; set; }
        public bool inserting { get; set; }
        public bool editing { get; set; }
        public bool deleting { get; set; }
        public bool sorting { get; set; }
        public bool autoload { get; set; }
        public bool pageLoading { get; set; }
        public bool paging { get; set; }//set to "false" to show all items in the grid

        public string noDataContent { get; set; }
        public string loadMessage { get; set; }
        public string deleteConfirm { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Number of items to show on the page
        /// </summary>
        public int pageSize { get; set; }
        public int pageButtonCount { get; set; }

        /// <summary>
        /// Rows
        /// </summary>
        public List<object> data { get; set; }
        public List<JSGridColumn> fields { get; set; }

        public string updateURL { get; set; }
        public string deleteURL { get; set; }
        public string insertURL { get; set; }

        public static List<object> GetDataAsObjects(IEnumerable<object> data)
        {
            return data?.Select(obj => obj as object).ToList() ?? new List<object>();
        }
    }
}