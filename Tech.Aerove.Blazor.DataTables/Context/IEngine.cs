using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    /// <summary>
    /// Used specifically to allow passing the context without a type parameter to
    /// prevent the user from needing to specify the item type in multiple places
    /// </summary>
    internal interface IEngine
    {
        /// <summary>
        /// Updates the Items then calls the event handler OnAfterUpdate
        /// To tell the listeners
        /// The Datatable component is a listener and calls Statehaschanged to update the ui
        /// </summary>
        Task UpdateAsync();
    }
}
