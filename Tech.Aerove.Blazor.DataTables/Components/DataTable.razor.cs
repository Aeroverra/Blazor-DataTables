using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Dynamic.Core;
using Tech.Aerove.Blazor.DataTables.Attributes;
using Tech.Aerove.Blazor.DataTables.Extensions;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class DataTable<TItem> : IDisposable
    {
        #region params

        //Render Templates
        [Parameter] public RenderFragment<TableSource<TItem>>? TableTop { get; set; }
        [Parameter] public RenderFragment<TableSource<TItem>>? TableTopLeft { get; set; }
        [Parameter] public RenderFragment<TableSource<TItem>>? TableTopRight { get; set; }
        [Parameter] public RenderFragment<TemplateTableHeadModel<TItem>>? TableHead { get; set; }
        [Parameter] public RenderFragment<TemplateTableBodyModel<TItem>>? TableBody { get; set; }


        [Parameter, AllowNull] public List<TItem> Items { get; set; } = new List<TItem>();
        [Parameter, AllowNull] public TableSource<TItem>? DataSource { get; set; }

        /// <summary>
        /// Table attributes that will be pasted to the html table element
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes { get; set; }
        #endregion

        internal TableData TableData = new TableData();

        internal List<ColumnInfoModel> Columns = new List<ColumnInfoModel>();

        protected override async Task OnInitializedAsync()
        {
            Columns = ColumnInfoModel.GetColumns<TItem>(TableData);
            TableData.Filters.PopulateFilterList(Columns);
            if (DataSource != null)
            {
                TableData.UpdateAsync = QueryDataAsync;
                DataSource.TableData = TableData;
                await QueryDataAsync();
            }

        }

        private SemaphoreSlim QueryLock = new SemaphoreSlim(1);
        private async Task QueryDataAsync()
        {
            if (DataSource == null) { return; }
            await QueryLock.WaitAsync();
            try
            {

                var query = DataSource.GetQuery();

                if (query is IAsyncQueryProvider)
                {
                    TableData.RecordsTotal = await query.CountAsync();
                }
                else
                {
                    TableData.RecordsTotal = query.Count();
                }


                //WARNING: This uses params to prevent SQL Injection!
                if (Columns.Searchable() && !string.IsNullOrWhiteSpace(TableData.SearchInput))
                {
                    List<string> searchStrings = new List<string>();
                    List<object> searchParams = new List<object>() { TableData.SearchInput };
                    foreach (var column in Columns.Where(x => x.SearchMode != Models.Enums.SearchMode.None))
                    {
                        if (column.Type.IsEnum)
                        {
                            var enumNames = column.Type.GetEnumNames();
                            var enumResults = new List<int>();
                            if (column.SearchMode == Models.Enums.SearchMode.Exact)
                            {
                                enumResults = enumNames
                                   .Where(x => x.ToLower() == TableData.SearchInput.ToLower())
                                   .Select(x => (int)Enum.Parse(column.Type, x))
                                   .ToList();
                            }
                            else
                            {
                                enumResults = enumNames
                                    .Where(x => x.ToLower().Contains(TableData.SearchInput.ToLower()))
                                    .Select(x => (int)Enum.Parse(column.Type, x))
                                    .ToList();
                            }

                            foreach (var enumResult in enumResults)
                            {
                                searchStrings.Add($"{column.Name} == {enumResult}");
                            }
                        }
                        else
                        {
                            var param = "@0";
                            if (column.SearchMode == Models.Enums.SearchMode.Exact)
                            {
                                searchStrings.Add($"{column.Name} == {param}");
                            }
                            else
                            {
                                searchStrings.Add($"{column.Name}.Contains({param})");
                            }
                        }

                    }
                    query = query.Where(string.Join(" || ", searchStrings), searchParams.ToArray());
                }
                query = TableData.Filters.AddToQuery(query);

                if (query is IAsyncQueryProvider)
                {
                    TableData.RecordsFiltered = await query.CountAsync();
                }
                else
                {
                    TableData.RecordsFiltered = query.Count();
                }

                query = TableData.OrderableCommands.OrderQuery(query);

                query = query.Skip((TableData.Page - 1) * TableData.Length);

                query = query.Take(TableData.Length);

                var result = await DataSource.FinishQueryAsync(query);

                Items.Clear();
                Items.AddRange(result);
                await InvokeAsync(() => StateHasChanged());
                QueryLock.Release();
            }
            catch (Exception e)
            {
                QueryLock.Release();
                throw new Exception("Failed to Query", e);
            }
        }

        private async Task OnLengthChangeAsync(ChangeEventArgs args)
        {
            var value = int.Parse($"{args.Value}");
            var lengths = DataLengthAttribute.GetLengths<TItem>();
            //Make sure the user isn't manipulating the values
            if (!lengths.Contains(value))
            {
                return;
            }
            TableData.Length = value;
            await TableData.UpdateAsync();
        }

        private void OnSearchChange(ChangeEventArgs args)
        {
            TableData.Search($"{args.Value}");
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