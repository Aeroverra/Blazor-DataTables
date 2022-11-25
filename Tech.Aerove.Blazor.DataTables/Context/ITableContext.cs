using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Api;
using Tech.Aerove.Blazor.DataTables.Configs;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    /// <summary>
    /// Used specifically to allow passing the context without a type parameter to
    /// prevent the user from needing to specify the item type in multiple places
    /// </summary>
    internal interface ITableContext
    {
        /// <summary>
        /// The central data processing location which handles the setup
        /// calls the overrides and manages the data
        /// </summary>
        internal IEngine Engine { get; }

        /// <summary>
        /// The api allowing control over table operations
        /// </summary>
        internal ITableController Api { get; }

        /// <summary>
        /// Configuration specific to the ui
        /// </summary>
        internal UIConfig UIConfig { get; }

        /// <summary>
        /// Configuration specific to the actual table
        /// </summary>
        internal RunningConfig RunningConfig { get; }

    }
}
