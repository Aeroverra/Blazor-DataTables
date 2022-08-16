namespace Tech.Aerove.Blazor.DataTables.Models
{
    internal class TableData
    {
        internal int Page { get; set; } = 1;
        internal int RecordsTotal { get; set; } = 0;
        internal int RecordsFiltered { get; set; } = 0;
        internal int Start { get; set; } = 0;
        internal int Length { get; set; } = 10;

        internal List<Func<IQueryable, IQueryable>> SearchCommands = new List<Func<IQueryable, IQueryable>>();
        internal List<Func<IQueryable, IQueryable>> OrderCommands = new List<Func<IQueryable, IQueryable>>();

        internal Func<Task>? UpdateAsync;


        internal void SetPage(int page)
        {
            Page = page;
            Start = (Page - 1) * Length;
        }
    }
}
