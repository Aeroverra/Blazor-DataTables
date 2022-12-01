using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Api
{
    public interface ITableController
    {
        event Func<Task> OnAfterUpdate;

        void OrderReset();
        void SwapOrder(string name, bool append = false);
        Task UpdateAsync(bool visualOnly = false);
    }
}
