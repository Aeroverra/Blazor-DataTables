using DataTable_Examples.Data;
using DataTable_Examples.Pages.Junk;
using DataTable_Examples.Pages.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tech.Aerove.Blazor.DataTables.Configs;
using Tech.Aerove.Blazor.DataTables.Context;
using Tech.Aerove.Blazor.DataTables.Enums;

namespace DataTable_Examples.Pages.Users
{
    public class TableSource : TableContext<IdentityUser>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _databaseFactory;
        private ApplicationDbContext? Context { get; set; }
        public TableSource(IDbContextFactory<ApplicationDbContext> databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        protected override async Task<IQueryable<IdentityUser>> OnStartQueryAsync()
        {
            Context = await _databaseFactory.CreateDbContextAsync();
            return Context.Users;
        }
        protected override void OnConfiguring(InitialConfig config)
        {
            config.DefaultFilterable = true;
            config.Column<IdentityUser>(x => x.Email).SearchMode(SearchMode.Like);
        }
    }
}
