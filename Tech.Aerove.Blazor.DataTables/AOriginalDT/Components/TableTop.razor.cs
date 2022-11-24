using Microsoft.AspNetCore.Components;
using Tech.Aerove.Blazor.DataTables.AOriginalDT.Attributes;
using Tech.Aerove.Blazor.DataTables.AOriginalDT.Models;

namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Components
{
    public partial class TableTop<TItem> : ComponentBase
    {
        /// <summary>
        /// Central communication Between all components
        /// Passed down from the DataTable Component
        /// </summary>
        [CascadingParameter] private TableNetwork<TItem> Network { get; set; } = null!;

        /// <summary>
        /// Called when the user changes the length via the selectlist
        /// </summary>
        private async Task OnLengthChangeAsync(ChangeEventArgs args)
        {
            var value = int.Parse($"{args.Value}");
            var lengths = DataLengthAttribute.GetLengths<TItem>();
            //Make sure the user isn't manipulating the values
            if (!lengths.Contains(value))
            {
                return;
            }
            Network.TableData.Length = value;
            await Network.TableData.UpdateAsync();
        }

        /// <summary>
        /// Called when the user changes the search text in the search input component
        /// </summary>
        private void OnSearchChange(ChangeEventArgs args)
        {
            Network.TableData.Search($"{args.Value}");
        }

    }
}