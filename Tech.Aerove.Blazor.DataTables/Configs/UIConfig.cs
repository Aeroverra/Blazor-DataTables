using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Configs
{
    internal class UIConfig
    {
        internal bool EnableLengthDropdown = true;
        internal bool EnableSearchBar = true;
        internal int[] Lengths = new int[] { 10, 25, 50, 100 };


        /// <summary>
        /// Sets values from Initial User config 
        /// This should only be called once in onconfiguring from the engine
        /// </summary>
        internal void Setup(InitialConfig config)
        {
            EnableLengthDropdown = config.UIEnableLengthDropdown;
            EnableSearchBar = config.UIEnableSearchBar;
            Lengths = config.UIDisplayedLengths;
        }
    }
}
