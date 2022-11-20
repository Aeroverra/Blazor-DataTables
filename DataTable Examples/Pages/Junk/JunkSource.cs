using DataTable_Examples.Data;
using DataTable_Examples.Pages.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Tech.Aerove.Blazor.DataTables.Models;

namespace DataTable_Examples.Pages.Junk
{
    public class JunkSource : TableSource<JunkModel>
    {

        private List<JunkModel> JunkList = new List<JunkModel>();

        public JunkSource()
        {
            for (var x = 0; x < 100; x++)
            {
                JunkList.Add(new JunkModel());
            }

        }
        public override void Dispose()
        {

        }

        public override async Task<List<JunkModel>> FinishQueryAsync(IQueryable query)
        {
            await Task.Delay(1);
            var queryable = query as IQueryable<JunkModel>;
            var result = queryable!.ToList();
            return result;
        }

        public override IQueryable GetQuery()
        {

            return JunkList.AsQueryable();
        }
    }
}
