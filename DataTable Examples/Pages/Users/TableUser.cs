using Microsoft.AspNetCore.Identity;

namespace DataTable_Examples.Pages.Users
{
    public class TableUser
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public TableUser(IdentityUser identityUser)
        {
            Id = identityUser.Id;
            Email = identityUser.Email;
            PasswordHash = identityUser.PasswordHash;
        }
    }
}
