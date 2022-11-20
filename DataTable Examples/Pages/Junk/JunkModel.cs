using Microsoft.AspNetCore.Identity;
using Tech.Aerove.Blazor.DataTables.Attributes;
using Tech.Aerove.Blazor.DataTables.Models;
using Tech.Aerove.Blazor.DataTables.Models.Enums;

namespace DataTable_Examples.Pages.Junk
{
    public class JunkModel
    {
        [DataSearch(SearchMode.Exact)]
        [DataFilterable(true)]
        [DataOrderable(true)]
        public int Id { get; set; }

        [DataFilterable(true)]
        public DateTime Date { get; set; }

        [DataSearch(SearchMode.Like)]
        [DataOrderable(true)]
        public Guid Guid { get; set; }

        public JunkModel()
        {
            Random random = new Random();
            Id = random.Next(0,10);
            Date = DateTime.Now.AddDays(random.Next(0, 10));
            Guid = Guid.NewGuid();
        }
    }
}
