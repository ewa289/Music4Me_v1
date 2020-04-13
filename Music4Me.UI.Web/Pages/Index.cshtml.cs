using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Music4Me.Client;
using Music4Me.Client.Resources;

namespace Music4Me.UI.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMusic4MeClient music4MeClient;

        public IList<Category> Categories { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }
        public IndexModel(IMusic4MeClient music4MeClient)
        {
            this.music4MeClient = music4MeClient;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try {
                this.Categories = await this.music4MeClient.GetCategories();
                ErrorMessage = null;
                return Page();

            } catch (Exception ex) {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
