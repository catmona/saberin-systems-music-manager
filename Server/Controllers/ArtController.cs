using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;


namespace music_manager_starter.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<string>> UploadAlbumArt([FromBody]string art64)
        {
            Console.WriteLine(art64);
            if (art64 == "" || art64 == null) {
                return BadRequest("Invalid image.");
            }


            try {
                var url = await uploadImgur(art64.Substring(art64.IndexOf(",") + 1));
                Console.WriteLine(url);
                return Ok(url);
            } 
            catch (Exception e) {
                return BadRequest("Image upload failed");
            }

            
        }

        private async Task<string> uploadImgur(string art64) {
            using var Http = new HttpClient();
            Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Client-ID", "30463bf70b5e7bf");

            var content = new MultipartFormDataContent {
                { new StringContent(art64), "image" }
            };

            var res = await Http.PostAsync("https://api.imgur.com/3/image", content);
            if (!res.IsSuccessStatusCode) throw new Exception("Imgur API Error: " + res.ReasonPhrase);

            var jsonResponse = await res.Content.ReadFromJsonAsync<ImgurApiResponse>();
            return jsonResponse?.Data?.Link ?? throw new Exception("Failed to retrieve Imgur url");

           
        }
    }


    public class ImgurApiResponse
    {
        public ImgurData Data { get; set; }
    }

    public class ImgurData
    {
        public string Link { get; set; }
    }
}
