using DataTable_Examples.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DataTable_Examples.Pages.Users
{
    public partial class UsersPage : ComponentBase
    {
        [Inject]
        private IDbContextFactory<ApplicationDbContext> _databaseFactory { get; set; } = null!;

        private TableSource Source = null!;
        protected override Task OnInitializedAsync()
        {
            Source = new TableSource(_databaseFactory);
            return Task.CompletedTask;
        }
    }
}