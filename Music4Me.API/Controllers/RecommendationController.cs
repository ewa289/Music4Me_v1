using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Music4Me.Application.Services;
using Music4Me.Client.Resources;
using Music4Me.Common.Extensions;
using Newtonsoft.Json.Linq;

namespace Music4Me.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class RecommendationController : ControllerBase
    {
        private readonly IMusic4MeService service;

        public RecommendationController(IMusic4MeService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("artists/{category}")]
        public async Task<ActionResult<IList<Artist>>> GetArtists(string category, string country = "se", int? maxCount = null)
        {
            try {
                return Ok(await this.service.GetArtists(category, country, maxCount));
            } catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("categories")]
        public async Task<ActionResult<IList<Category>>> GetCategories(string country = "se")
        {
            try {
                return Ok(await this.service.GetCategories(country));
            } catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("recommendations")]
        public async Task<ActionResult<IList<Artist>>> GetRecommendations(Selections selections, int? maxCount = null)
        {
            try {
                if (selections == null 
                    || selections.LikedArtitsts == null 
                    || !selections.LikedArtitsts.Any()) {
                    throw new ArgumentNullException("Failed to get recommendations. Selected artists missing.");
                }

                return Ok(await this.service.GetRecommendedArtists(selections, maxCount));
            } catch (Exception ex) {
                return BadRequest(ex);
            }
        }
    }
}
