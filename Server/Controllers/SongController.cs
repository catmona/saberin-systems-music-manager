using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_manager_starter.Data;
using music_manager_starter.Data.Models;
using System;

namespace music_manager_starter.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly DataDbContext _context;

        public SongsController(DataDbContext context)
        {
            _context = context;
        }

  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            return await _context.Songs.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Song>> PostSong(Song song)
        {
            if (song == null)
            {
                return BadRequest("Song cannot be null.");
            }


            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("imgur-art")]
        public async Task<ActionResult<string>> UploadAlbumArt(string art64)
        {
            if (art64 == "" || art64 == null) {
                return BadRequest("Invalid image.");
            }

            using var Http = new HttpClient();

            Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Client-ID", "30463bf70b5e7bf");

            var content = new MultipartFormDataContent {
                { new StringContent(art64), "image" }
            };

            var res = await Http.PostAsync("https://api.imgur.com/3/image", content);
            if (!res.IsSuccessStatusCode) throw new Exception("Imgur API Error: " + res.ReasonPhrase);

            var json = await res.Content.ReadFromJsonAsync<string>();
            Console.WriteLine(json);
            return null;
        }
    }
}
