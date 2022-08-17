using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;
using Tech.Aerove.Blazor.DataTables.Models;
using Tech.Aerove.Blazor.DataTables.Attributes;
using Tech.Aerove.Blazor.DataTables.Extensions;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class DataTable<TItem> : IDisposable
    {
        [Parameter] public RenderFragment<TableHeadColumn<TItem>>? TableHead { get; set; }
        [Parameter] public RenderFragment<TableRowColumn<TItem>>? TableBody { get; set; }
        [Parameter, AllowNull] public List<TItem> Items { get; set; } = new List<TItem>();
        [Parameter, AllowNull] public TableSource<TItem>? DataSource { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes { get; set; }

        internal TableData TableData = new TableData();

        public List<PropertyInfo> Columns = new List<PropertyInfo>();


        protected override async Task OnInitializedAsync()
        {
            SetColumns();
            if (DataSource != null)
            {
                TableData.UpdateAsync = QueryDataAsync;
                DataSource.TableData = TableData;
                await QueryDataAsync();
            }

        }

        private async Task QueryDataAsync()
        {
            if (DataSource == null) { return; }

            var query = DataSource.GetQuery();

            TableData.RecordsTotal = await query.CountAsync();
            foreach (var searchCommand in TableData.SearchCommands)
            {
                query = searchCommand(query);
            }
            TableData.RecordsFiltered = await query.CountAsync();

            query = TableData.Orderables.OrderQuery(query);

            query = query.Skip((TableData.Page - 1) * TableData.Length);

            query = query.Take(TableData.Length);

            var result = await DataSource.FinishQueryAsync(query);
            Items.Clear();
            Items.AddRange(result);
            StateHasChanged();
        }

        private void SetColumns()
        {

            var defaultRender = true;
            var type = typeof(TItem);
            var dataTable = type.GetCustomAttribute<DataTableAttribute>();
            if (dataTable != null)
            {
                defaultRender = dataTable.DefaultRender;
            }
            List<PropertyInfo> columns = type.GetProperties().ToList();

            foreach (var column in columns)
            {
                var render = defaultRender;
                var columnAttribute = column.GetCustomAttribute<DataColumnAttribute>();
                if (columnAttribute != null && columnAttribute.Render != null)
                {
                    render = columnAttribute.Render.Value;
                }
                if (render)
                {
                    Columns.Add(column);
                }
            }
        }

        public void Dispose()
        {
            if (DataSource != null)
            {
                DataSource.Dispose();
            }
        }
    }
}