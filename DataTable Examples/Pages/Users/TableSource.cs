using DataTable_Examples.Data;
using DataTable_Examples.Pages.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tech.Aerove.Blazor.DataTables.AOriginalDT.Models.Abstracts;

namespace DataTable_Examples.Pages.Users
{
    public class TableSource : TableSource<TableUser>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _databaseFactory;
        private ApplicationDbContext? Context { get; set; }
        public TableSource(IDbContextFactory<ApplicationDbContext> databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public override void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }

        public override async Task<List<TableUser>> FinishQueryAsync(IQueryable query)
        {
            var queryable = query as IQueryable<IdentityUser>;
            var result = await queryable!.ToListAsync();
            return result.Select(x => new TableUser(x)).ToList();
        }

        public override IQueryable GetQuery()
        {
            Context = _databaseFactory.CreateDbContext();
            var query = Context.Users;

            return query;
        }
    }
}
