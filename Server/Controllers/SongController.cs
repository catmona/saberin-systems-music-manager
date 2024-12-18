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
        private readonly ILogger<SongsController> _logger;

        public SongsController(DataDbContext context, ILogger<SongsController> logger)
        {
            _context = context;
            _logger = logger;
        }

  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            _logger.LogInformation("Get Songs request received");
            return await _context.Songs.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Song>> PostSong(Song song)
        {
            _logger.LogInformation("Add new song request received");
            if (song == null)
            {
                _logger.LogInformation("Song was null");
                return BadRequest("Song cannot be null.");
            }


            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Song added to db: " + song.Title);

            return Ok();
        }

    }
}
