using BioTapMedical.Models.JSGridModels.JSGridColumns.Abstracts;
using System.Collections.Generic;
using System.Linq;

namespace BioTapMedical.Models.JSGridModels
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

        private void SetupDefaults()
        {
            WidthAsIntegerPercentage = 100;

            pageSize = 10;
            pageButtonCount = 5;

            filtering = true;
            sorting = true;
            paging = true;
            loadMessage = "Please, wait...";
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
        public bool paging { get; set; }//set to "false" to show all items in the grid

        public string noDataContent { get; set; }
        public string loadMessage { get; set; }
        public string deleteConfirm { get; set; }

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