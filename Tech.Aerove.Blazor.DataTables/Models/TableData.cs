namespace Tech.Aerove.Blazor.DataTables.Models
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

        internal event EventHandler? OnOrderReset;
        internal Func<Task>? UpdateAsync;

        internal void ResetOrder()
        {
            OrderableCommands.Clear();
            OnOrderReset?.Invoke(this, EventArgs.Empty);
        }

        internal Task SetLengthAsync(int length)
        {
            Length = length;
            if (UpdateAsync != null)
            {
                return UpdateAsync();
            }
            return Task.CompletedTask;
        }

        internal Task SetPageAsync(int page)
        {
            Page = page;
            Start = (Page - 1) * Length;
            if (UpdateAsync != null)
            {
                return UpdateAsync();
            }
            return Task.CompletedTask;
        }

        internal Task SetPageNextAsync() => SetPageAsync(Page + 1);

        internal Task SetPagePreviousAsync() => SetPageAsync(Page - 1);

        internal Task SetPageFirstAsync() => SetPageAsync(1);

        internal Task SetPageLastAsync() => SetPageAsync(TotalPages);

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
