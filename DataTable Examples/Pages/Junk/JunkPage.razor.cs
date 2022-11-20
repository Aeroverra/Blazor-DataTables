using DataTable_Examples.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DataTable_Examples.Pages.Junk
{
    public partial class JunkPage : ComponentBase
    {
        [Inject]
        private IDbContextFactory<ApplicationDbContext> _databaseFactory { get; set; } = null!;

        private JunkSource? Source;
        protected override Task OnInitializedAsync()
        {
            Source = new JunkSource();
            return Task.CompletedTask;
        }
    }
}