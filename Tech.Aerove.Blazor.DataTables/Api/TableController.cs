using Tech.Aerove.Blazor.DataTables.Configs;
using Tech.Aerove.Blazor.DataTables.Context;
using Tech.Aerove.Blazor.DataTables.Enums;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Api
{
    /// <summary>
    /// The main api for controlling the table
    /// </summary>
    public class TableController : ITableController
    {
        private readonly ITableContext Context;

        public TableController(ITableContext tableContext)
        {
            Context = tableContext;
        }

        /// <summary>
        /// Tells listeners we have fresh data. The datatable component uses
        /// this to call statehaschanged and update the ui
        /// </summary>
        public event Func<Task> OnAfterUpdate = null!;

        /// <summary>
        /// Tells listeners the datatable ui is updated. This is called by the datatable
        /// during onafterupdateasync is invoked to let the listeners knows the component
        /// has finished rendering possible new items. Usually in conjunction with javascript
        /// calls to rerender small things like icons.
        /// </summary>
        public event Func<Task>? OnAfterUpdateRendered = null;

        /// <summary>
        /// length of records queried and displayed
        /// </summary>
        public int Length
        {
            get => RunningConfig.Length;
            set
            {
                if (value > RunningConfig.MaxLength)
                {
                    throw new ArgumentException($"Maxiumum length of {value} can not be set due to its constraints.");
                }
                RunningConfig.Length = value;
                Page = 1;
            }
        }

        public int Page
        {
            get => RunningConfig.Page;
            set
            {
                RunningConfig.Page = value;
                RunningConfig.Start = (Page - 1) * Length;
            }
        }

        public int RecordsFiltered => RunningConfig.RecordsFiltered;
        public int RecordsTotal => RunningConfig.RecordsTotal;

        /// <summary>
        /// Search text
        /// </summary>
        public string? SearchText
        {
            get => RunningConfig.SearchText;
            set
            {
                RunningConfig.SearchText = $"{value}";
                Page = 1;
            }
        }

        public int Start => RunningConfig.Start;
        public int TotalPages => RecordsFiltered / Length + (RecordsFiltered % Length > 0 ? 1 : 0);
        private IEngine Engine => Context.Engine;
        private RunningConfig RunningConfig => Context.RunningConfig;
        private UIConfig UIConfig => Context.UIConfig;

        /// <summary>
        /// Resets all filters
        /// </summary>
        public void FilterReset()
        {
            //Get all columns enabled
            var columns = RunningConfig.Columns
                .Where(x => x.Filterable)
                .ToList();

            //clear filters for each direction
            foreach (var column in columns)
            {
                column.Filters.Clear();
            }
        }

        /// <summary>
        /// Gets the applied filters for the specified column
        /// </summary>
        /// <param name="name">Case sensitive column name</param>
        public List<string> GetFilters(string name)
        {
            var column = GetColumn(name);
            return GetFilters(column);
        }

        /// <summary>
        /// Handles filter changes from the ui
        /// This will requery the table results
        /// </summary>
        /// <param name="name">Case sensitive column name</param>
        /// <param name="e"></param>
        public Task HandleFilterChangeAsync(string name, ChangeEventArgs args)
        {
            object? value = args.Value;
            if (value == null)
            {
                FilterReset();
                return UpdateAsync();
            }
            if (value.GetType() == typeof(string))
            {
                if (string.IsNullOrEmpty($"{value}"))
                {
                    return UpdateAsync();
                }
                SetFilter(name, $"{value}");
                return UpdateAsync();
            }
            if (value.GetType() == typeof(string[]))
            {
                //Remove null / empty to allow default value
                var arr = ((string[])value)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();

                SetFilters(name, arr);
                return UpdateAsync();
            }

            throw new Exception($"Object of type {value.GetType()} for field {name} not supported for filtering");
        }

        /// <summary>
        /// Resets all column ordering
        /// </summary>
        public void OrderReset()
        {
            //Get all columns enabled
            var columns = RunningConfig.Columns
                .Where(x => x.OrderableDirection != OrderableDirection.Disabled)
                .ToList();

            //reset direction
            foreach (var column in columns)
            {
                column.OrderableDirection = OrderableDirection.None;
            }

            //clear ordered list
            RunningConfig.ColumnsOrdered.Clear();
        }

        /// <summary>
        /// Sets the filter of a column
        /// </summary>
        /// <param name="name">Case sensitive column name</param>
        /// <param name="values">filters to apply</param>
        /// <param name="append">If set to true will add to the current column filters</param>
        public void SetFilter(string name, string value, bool append = false)
        {
            SetFilters(name, new string[] { value }, append);
        }

        /// <summary>
        /// Sets the filters of a column
        /// </summary>
        /// <param name="name">Case sensitive column name</param>
        /// <param name="values">filters to apply</param>
        /// <param name="append">If set to true will add to the current column filters</param>
        public void SetFilters(string name, string[] values, bool append = false)
        {
            var column = GetColumn(name);
            SetFilters(column, values, append);
        }

        public void SetPageFirst()
        {
            Page = 1;
        }

        public void SetPageLast()
        {
            Page = TotalPages;
        }

        public void SetPageNext()
        {
            Page = Page + 1;
        }

        public void SetPagePrevious()
        {
            Page = Page - 1;
        }

        /// <summary>
        /// Swaps the ordering direction of a column
        /// </summary>
        /// <param name="name">Case sensitive column name</param>
        /// <param name="append">If set to true will add a second order property like .ThenBy()</param>
        /// <exception cref="ArgumentException"></exception>
        public void SwapOrder(string name, bool append = false)
        {
            var column = GetColumn(name);
            SwapOrder(column, append);
        }

        /// <summary>
        /// Updates the Items then calls the event handler OnAfterUpdate
        /// To tell the listeners
        /// The Datatable component is a listener and calls Statehaschanged to update the ui
        /// </summary>
        /// <param name="visualOnly">Updates the ui without requerying the data</param>
        /// <returns></returns>
        public async Task UpdateAsync(bool visualOnly = false)
        {
            if (visualOnly == false)
            {
                //call our engine to query the data again
                await Engine.UpdateAsync();
            }

            //call our event handler so listeners can perform actions
            //like the datatable component which will call statehaschanged
            await OnAfterUpdate();
        }

        /// <summary>
        /// Gets column by name
        /// </summary>
        /// <param name="name">case sensitive name</param>
        /// <exception cref="ArgumentException"></exception>
        internal ColumnModel GetColumn(string name)
        {
            var column = RunningConfig.Columns.FirstOrDefault(x => x.Name == name);
            return column == null ? throw new ArgumentException($"Could not find column with name {name}.") : column;
        }

        /// <summary>
        /// Gets the applied filters for the specified column
        /// </summary>
        /// <param name="column">Column</param>
        internal List<string> GetFilters(ColumnModel column)
        {
            return column.Filters.ToList();
        }

        /// <summary>
        /// Sets the filters of a column
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="values"></param>
        /// <param name="append">If set to true will add to the current column filters</param>
        internal void SetFilters(ColumnModel column, string[] values, bool append = false)
        {
            if (column.Filterable == false)
            {
                throw new ArgumentException($"This column has filtering disabled and can not be changed!");
            }

            if (append == false)
            {
                column.Filters.Clear();
            }
            column.Filters.AddRange(values);
        }

        /// <summary>
        /// Swaps the ordering direction of a column
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="append">If set to true will add a second order property like .ThenBy()</param>
        /// <exception cref="ArgumentException"></exception>
        internal void SwapOrder(ColumnModel column, bool append = false)
        {
            if (column.OrderableDirection == OrderableDirection.Disabled)
            {
                throw new ArgumentException($"This column has been disabled and can not be changed!");
            }
            //get current direction so we can determine the next direction
            var currentDirection = column.OrderableDirection;
            if (append == false)
            {
                //resets ordering in case other columns were set before this
                OrderReset();
            }
            else
            {
                //append is true so so if this column is already in the orderables
                //remove it so we don't make a duplicate.
                //append usually used in cases like the ui where shift is held down
                RunningConfig.ColumnsOrdered.RemoveAll(x => x == column);
            }
            //switch on current direction saved before the reset and set new column value
            switch (currentDirection)
            {
                case OrderableDirection.None: column.OrderableDirection = OrderableDirection.Ascending; break;
                case OrderableDirection.Ascending: column.OrderableDirection = OrderableDirection.Descending; break;
                case OrderableDirection.Descending: column.OrderableDirection = OrderableDirection.None; break;
            }

            // Add it to our ordering list so we know which one is ordered by 1st, 2nd, 3rd etc...
            // but only if the direction is not none / neutral
            if (column.OrderableDirection != OrderableDirection.None)
            {
                RunningConfig.ColumnsOrdered.Add(column);
            }
        }

        /// <summary>
        /// Called by the DataTable component to let us know we should invoke the 
        /// OnAfterUpdateRendered event listener. This is invoked when datatatables
        /// onafterupdateasync is invoked to let the listeners knows the component
        /// has finished rendering possible new items. 
        /// </summary>
        internal Task AfterUpdateRenderedAsync()
        {
            if (OnAfterUpdateRendered != null)
            {
                return OnAfterUpdateRendered();
            }
            return Task.CompletedTask;
        }
    }
}