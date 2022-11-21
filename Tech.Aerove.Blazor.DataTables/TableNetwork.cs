namespace Tech.Aerove.Blazor.DataTables
{
    internal class TableNetwork<TItem> : TableNetwork
    {
        public List<TItem> Items { get; set; } = new List<TItem>();

        public TableNetwork()
        {
            Columns = ColumnInfoModel.GetColumns<TItem>();
            TableData.Filters.PopulateFilterList(Columns);
        }
    }
    internal class TableNetwork
    {
        protected TableNetwork() { }

        protected internal List<ColumnInfoModel> Columns { get; protected set; } = new List<ColumnInfoModel>();

        internal TableData TableData { get; private set; } = new TableData();

        /// <summary>
        /// The catch all parameter with attributes put on the DataTable Component by the user which will be added
        /// to the table html ie: class="" id="" etc
        /// </summary>
        internal Dictionary<string, object>? InputAttributes { get; set; }



    }

}
