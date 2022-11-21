using Microsoft.AspNetCore.Identity;
using Tech.Aerove.Blazor.DataTables.Attributes;
using Tech.Aerove.Blazor.DataTables.Models;
using Tech.Aerove.Blazor.DataTables.Models.Enums;

namespace DataTable_Examples.Pages.Junk
{
    public class JunkModel
    {

        [DataFilterable(true)]
        [DataOrderable(true)]
        public int Id { get; set; }

        [DataFilterable(true)]
        public DateTime Date { get; set; }

        [DataOrderable(true)]
        public Guid Guid { get; set; }

        [DataOrderable(true)]
        public Guid Guid2 { get; set; }

        [DataOrderable(true)]
        public Guid Guid3 { get; set; }

        [DataOrderable(true)]
        [DataSearch(SearchMode.Like)]
        public string Email { get; set; }

        [DataOrderable(true)]
        [DataSearch(SearchMode.Like)]
        public string Phone { get; set; }

        public JunkModel()
        {
            var emails = new string[] { "testemail@test.com", "no-reply@autobuy.io", "pot@honey.com" };
            var phones = new string[] { "333-333-3333", "123-456-7890", "800-850-0000" };
            Random random = new Random();
            Id = random.Next(0, 10);
            Date = DateTime.Now.AddDays(random.Next(0, 10));
            Guid = Guid.NewGuid();
            Guid2 = Guid.NewGuid();
            Guid3 = Guid.NewGuid();
            Email = emails[random.Next(0, 3)];
            Phone = phones[random.Next(0, 3)];
        }
    }
}
