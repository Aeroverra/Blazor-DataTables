﻿namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Models.Abstracts
{
    public abstract class TableSource<TItem> : IDisposable
    {
        public abstract IQueryable GetQuery();

        public abstract Task<List<TItem>> FinishQueryAsync(IQueryable query);

        public abstract void Dispose();

        internal TableData? TableData { get; set; }


        /// <summary>
        /// Forces a requery
        /// </summary>
        /// <returns></returns>
        public Task UpdateAsync()
        {
            CheckSource();
            return TableData.UpdateAsync();
        }

        /// <summary>
        /// Sets the search text and performs a search
        /// Automatically delays for input
        /// </summary>
        /// <param name="text"></param>
        public void Search(string text)
        {
            CheckSource();
            TableData?.Search(text);
        }

        /// <summary>
        /// Resets all the column orders
        /// </summary>
        /// <returns></returns>
        public async Task ResetOrderAsync()
        {
            TableData?.ResetOrder();
            await UpdateAsync();
        }

        /// <summary>
        /// Sets the length of rows displayed
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public Task SetLengthAsync(int length)
        {
            CheckSource();
            return TableData.SetLengthAsync(length);
        }

        /// <summary>
        /// Set the page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Task SetPageAsync(int page)
        {
            CheckSource();
            return TableData.SetPageAsync(page);
        }

        /// <summary>
        /// This can be hooked to an onchange attached to a select list
        /// null data or empty string will remove the filter
        /// </summary>
        /// <param name="columnName">exact database column name</param>
        /// <param name="data">a string or a list of strings</param>
        /// <returns></returns>
        public Task SetFilterAsync(string columnName, object? data)
        {
            CheckSource();
            TableData.Filters.SetFilter(columnName, data);
            return UpdateAsync();
        }

        private void CheckSource()
        {
            if (TableData == null || TableData.UpdateAsync == null)
            {
                throw new Exception("Update failed.. not linked to any table.");
            }
        }

    }
}