using Microsoft.AspNetCore.Identity;

namespace DataTable_Examples.Pages.Junk
{
    public class JunkModel
    {
        public int Id { get; set; }


        public DateTime Date { get; set; }


        public Guid Guid { get; set; }


        public Guid Guid2 { get; set; }


        public Guid Guid3 { get; set; }


        public string Email { get; set; }


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
