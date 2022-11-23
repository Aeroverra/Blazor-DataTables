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
    /// <summary>
    /// Main Component which handles the initial instantiation of the sub components and shared data
    /// </summary>
    /// <typeparam name="TItem">The Object type to be queried</typeparam>
    public partial class DataTable<TItem> : ComponentBase, IDisposable
    {
        /// <summary>
        /// The Table Head render template. If not specified no table heade will be generated
        /// </summary>
        [Parameter] public RenderFragment? TableHead { get; set; }

        /// <summary>
        /// The Table Body render template. If not specified no table heade will be generated
        /// </summary>
        [Parameter] public RenderFragment<TemplateTableBodyModel<TItem>>? TableBody { get; set; }

        /// <summary>
        /// The Source of the data which will be queries. Defined by the User
        /// </summary>
        [Parameter, AllowNull] public TableSource<TItem> DataSource { get; set; }

        /// <summary>
        /// Table attributes that will be pasted to the html table element
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? InputAttributes { get; set; }

        /// <summary>
        /// Central communication Between all components
        /// </summary>
        internal TableNetwork<TItem> Network = new TableNetwork<TItem>();

        protected override void OnParametersSet()
        {
            Network.InputAttributes = InputAttributes;
        }

        protected override async Task OnInitializedAsync()
        {
            Network.TableData.UpdateAsync = QueryDataAsync;
            DataSource.TableData = Network.TableData;
            await QueryDataAsync();

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
                    Network.TableData.RecordsTotal = await query.CountAsync();
                }
                else
                {
                    Network.TableData.RecordsTotal = query.Count();
                }


                //WARNING: This uses params to prevent SQL Injection!
                if (Network.Columns.Searchable() && !string.IsNullOrWhiteSpace(Network.TableData.SearchInput))
                {
                    List<string> searchStrings = new List<string>();
                    List<object> searchParams = new List<object>() { Network.TableData.SearchInput };
                    foreach (var column in Network.Columns.Where(x => x.SearchMode != Models.Enums.SearchMode.None))
                    {
                        if (column.Type.IsEnum)
                        {
                            var enumNames = column.Type.GetEnumNames();
                            var enumResults = new List<int>();
                            if (column.SearchMode == Models.Enums.SearchMode.Exact)
                            {
                                enumResults = enumNames
                                   .Where(x => x.ToLower() == Network.TableData.SearchInput.ToLower())
                                   .Select(x => (int)Enum.Parse(column.Type, x))
                                   .ToList();
                            }
                            else
                            {
                                enumResults = enumNames
                                    .Where(x => x.ToLower().Contains(Network.TableData.SearchInput.ToLower()))
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
                query = Network.TableData.Filters.AddToQuery(query);

                if (query is IAsyncQueryProvider)
                {
                    Network.TableData.RecordsFiltered = await query.CountAsync();
                }
                else
                {
                    Network.TableData.RecordsFiltered = query.Count();
                }

                query = Network.TableData.OrderableCommands.OrderQuery(query);

                query = query.Skip((Network.TableData.Page - 1) * Network.TableData.Length);

                query = query.Take(Network.TableData.Length);

                var result = await DataSource.FinishQueryAsync(query);

                Network.Items.Clear();
                Network.Items.AddRange(result);
                await InvokeAsync(() => StateHasChanged());
                QueryLock.Release();
            }
            catch (Exception e)
            {
                QueryLock.Release();
                throw new Exception("Failed to Query", e);
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine("Complete");
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