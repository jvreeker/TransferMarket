using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayersApi.Models;

namespace PlayersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly TransfermarketContext _context;

        public PlayersController(TransfermarketContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all players
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Get api/Players/
        /// </remarks>
        /// <returns>Returns players</returns>
        /// <response code="200">Ok</response>     
        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        /// <summary>
        /// Get Player information
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Get api/Players/1
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Returns player info</returns>
        /// <response code="200">Ok</response>        
        /// <response code="404">Player is not found</response>  
        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(long id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        /// <summary>
        /// Update player information.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/Players/2
        ///     {        
        ///         "id": 2,
        ///         "name": "Frenkie de jong",
        ///         "height": 180,
        ///         "birthday": "1997-05-12T00:00:00",
        ///         "avatarURL": "https://www.frenkiedejong.com/wp-content/uploads/2019/06/FD21_nieuwesite-1024x1024.jpg",
        ///         "teamId": null
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">Player is updated</response>
        /// <response code="400">Param id is different from the body</response>     
        /// <response code="404">Player is not found</response>     
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(long id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        /// <summary>
        /// Save an new player
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Players/
        ///     {
        ///     {
        ///   "name": "Frenkie de jong",
        ///   "height": 180,
        ///   "birthday": "1997-05-12T00:00:00",
        ///   "avatarURL": "https://www.frenkiedejong.com/wp-content/uploads/2019/06/FD21_nieuwesite-1024x1024.jpg",
        ///   "teamId": null
        ///     }
        /// </remarks>        
        /// <returns>Returns newly created player</returns>
        /// <response code="200">Ok</response>      
        /// <response code="404">Player is not found</response>      
        // POST: api/Players
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            _context.Players.Add(player);

            if (player.TeamId != null)
            {
                var team = await _context.Teams.FindAsync(player.TeamId);
                if (team == null)
                {
                    return NotFound();
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        /// <summary>
        /// Delete a player
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/Players/2
        ///    
        /// </remarks>    
        /// <param name="id"></param>
        /// <response code="200">Player is removed</response>      
        /// <response code="404">If player is not found</response>    
        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(long id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return player;
        }

        /// <summary>
        /// Connect a player to a team
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/Players/2/Team/1
        ///    
        /// </remarks>    
        /// <param name="id"></param>
        /// /// <param name="teamid"></param>
        /// <response code="200">Player is connect to a team</response>      
        /// <response code="404">If player is not found or team is not found</response>  
        /// 
        [HttpPut("{id}/Team/{teamid}")]
        public async Task<IActionResult> AddPlayerToTeam(long id, long teamid)
        {
            var team = await _context.Teams.FindAsync(teamid);
            if (team == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            player.TeamId = id;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool PlayerExists(long id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
