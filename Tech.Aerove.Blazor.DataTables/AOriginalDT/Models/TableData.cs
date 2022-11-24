namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Models
{
    internal class TableData
    {
        internal int Page { get; set; } = 1;
        internal int RecordsTotal { get; set; } = 0;
        internal int RecordsFiltered { get; set; } = 0;
        internal int Start { get; set; } = 0;
        internal int Length { get; set; } = 10;
        internal int TotalPages => (RecordsFiltered / Length + (RecordsFiltered % Length > 0 ? 1 : 0));
        internal string SearchInput { get; set; } = "";
        internal List<OrderCommand> OrderableCommands = new List<OrderCommand>();
        internal List<FilterModel> Filters = new List<FilterModel>();
        internal event EventHandler? OnOrderReset;
        internal Func<Task>? UpdateAsync;

        internal TableData()
        {
            SearchTimer = new Timer(Search);
        }

        internal void ResetOrder()
        {
            OrderableCommands.Clear();
            OnOrderReset?.Invoke(this, EventArgs.Empty);
        }

        #region search
        private readonly Timer SearchTimer;

        public void Search(string text)
        {
            SearchInput = text;
            SearchTimer.Change(400, Timeout.Infinite);
        }

        private async void Search(object? state)
        {
            if (UpdateAsync != null)
            {
                await UpdateAsync();
            }
        }
        #endregion

        internal Task SetLengthAsync(int length)
        {
            Length = length;
            return UpdateAsync != null ? UpdateAsync() : Task.CompletedTask;
        }

        internal Task SetPageAsync(int page)
        {
            Page = page;
            Start = (Page - 1) * Length;
            return UpdateAsync != null ? UpdateAsync() : Task.CompletedTask;
        }

        internal Task SetPageNextAsync()
        {
            return SetPageAsync(Page + 1);
        }

        internal Task SetPagePreviousAsync()
        {
            return SetPageAsync(Page - 1);
        }

        internal Task SetPageFirstAsync()
        {
            return SetPageAsync(1);
        }

        internal Task SetPageLastAsync()
        {
            return SetPageAsync(TotalPages);
        }

        internal string GetResultdescriptor()
        {
            var to = Start + Length;
            if (RecordsFiltered < to)
            {
                to = RecordsFiltered;
            }
            var descriptor = $"Showing {Start + 1} to {to} of {RecordsFiltered} entries";
            if (RecordsFiltered != RecordsTotal)
            {
                descriptor += $"(filtered from {RecordsTotal} total entries)";
            }
            return descriptor;
        }
    }
}
