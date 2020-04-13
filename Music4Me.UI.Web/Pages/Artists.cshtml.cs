using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Music4Me.Client;
using Music4Me.Client.Resources;

namespace Music4Me.UI.Web.Pages
{
    public class ArtistsModel : PageModel
    {
        private readonly IMusic4MeClient music4MeClient;

        [BindProperty]
        public IList<Artist> Artists { get; set; }

        [BindProperty]
        public string CategoryName { get; set; }

        [BindProperty]
        public IList<string> SelectedArtists { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }

        public ArtistsModel(IMusic4MeClient music4MeClient)
        {
            this.music4MeClient = music4MeClient;
        }
        public async Task OnGetAsync()
        {
            try {
                var selectedCategory = Request.Query["categoryId"][0];

                CategoryName = Request.Query["categoryName"][0];
                Artists = await this.music4MeClient.GetArtists(selectedCategory);
                ErrorMessage = null;
            } catch (Exception ex) {
                ErrorMessage = ex.Message;
            }
        }
    }
}