using Microsoft.AspNetCore.Components;

namespace DataTable_Examples.Pages.Junk
{

    public partial class NewJunk : ComponentBase
    {
        private NewJunkContext Source = null!;
        protected override Task OnInitializedAsync()
        {
            Source = new NewJunkContext();
            return Task.CompletedTask;
        }
        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine("Complete22222");
        }

    }
}