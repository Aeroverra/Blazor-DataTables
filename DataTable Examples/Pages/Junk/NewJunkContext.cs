using Tech.Aerove.Blazor.DataTables.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataTable_Examples.Pages.Junk
{
    public class NewJunkContext : TableContext<JunkModel>
    {
        private List<JunkModel> JunkList = new List<JunkModel>();

        public NewJunkContext()
        {
            for (var x = 0; x < 100; x++)
            {
                JunkList.Add(new JunkModel());
            }

        }
        protected override Task<IQueryable<JunkModel>> OnBeforeFinishQueryAsync(IQueryable<JunkModel> query)
        {
            return Task.FromResult(query);
        }

        protected override Task<IQueryable<JunkModel>> OnStartQueryAsync()
        {
            var query = JunkList.AsQueryable();
            return Task.FromResult(query);
        }
    }
}
