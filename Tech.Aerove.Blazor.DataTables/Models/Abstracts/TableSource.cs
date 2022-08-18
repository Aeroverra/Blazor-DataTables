namespace Tech.Aerove.Blazor.DataTables.Models
{
    public abstract class TableSource<TItem> : IDisposable
    {
        public abstract IQueryable GetQuery();


        public abstract Task<List<TItem>> FinishQueryAsync(IQueryable query);

        public abstract void Dispose();



        internal TableData? TableData { get; set; }

        public Task UpdateAsync()
        {
            if (TableData == null || TableData.UpdateAsync == null)
            {
                throw new Exception("Update failed.. not linked to any table.");
            }
            return TableData.UpdateAsync();
        }


    }
}
