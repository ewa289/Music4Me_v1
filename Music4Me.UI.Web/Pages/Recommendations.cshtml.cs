using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Music4Me.Client;
using Music4Me.Client.Resources;

namespace Music4Me.UI.Web.Pages
{
    public class RecommendationsModel : PageModel
    {
        private readonly IMusic4MeClient music4MeClient;

        [BindProperty]
        public IList<string> SelectedArtists { get; set; }

        [BindProperty]
        public string SelectionBase { get; set; }

        public IList<Artist> Recommendations { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }

        public RecommendationsModel(IMusic4MeClient music4MeClient)
        {
            this.music4MeClient = music4MeClient;
        }

        public async Task OnPostGetRecommendationsAsync()
        {
            try {
                Recommendations = await this.music4MeClient.GetRecommendations(
                       new Selections {
                           LikedArtitsts = SelectedArtists,
                           DislikedArtitsts = GetDislikedArtists()
                       });
                ErrorMessage = null;
            } catch (Exception ex) {
                ErrorMessage = ex.Message;
            }
        }

        private IList<string> GetDislikedArtists()
        {
            var allArtists = SelectionBase.Split(',');
            return allArtists.Except(SelectedArtists).ToList();
        }
    }
}