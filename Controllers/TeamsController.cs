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
    public class TeamsController : ControllerBase
    {
        private readonly TransfermarketContext _context;

        public TeamsController(TransfermarketContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Get all teams
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Get api/Teams/
        /// </remarks>
        /// <returns>Returns teams</returns>
        /// <response code="200">Ok</response>        
        
        // GET: api/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams.Include(x => x.Players).ToListAsync();
        }

        /// <summary>
        /// Get Team information
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Get api/Teams/1
        /// </remarks>
        /// <param name="id"></param>
         /// <returns>Returns team info</returns>
        /// <response code="200">Ok</response>        
        /// <response code="404">Team is not found</response>     
        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(long id)
        {
            //   var team = await _context.Teams.FindAsync(id);
            var team = _context.Teams.Include(x => x.Players).Single(x => x.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return team;
        }


        /// <summary>
        /// Update team information.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/Teams/5
        ///     {        
        ///       "id": 5,
        ///        "name": "Liverpol",        
        ///        "avatarURL": "https://commons.wikimedia.org/wiki/File:600px_Rosso_con_grifone_Bianco_scudato_e_fiamme.png"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">If team is updated</response>
        /// <response code="400">If the param id is different from the body</response>     
        /// <response code="404">If team is not found</response>     
        // PUT: api/Teams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(long id, Team team)
        {
            if (id != team.Id)
            {
                return BadRequest();
            }

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }


        /// <summary>
        /// Save an new team
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Post api/Teams/
        ///     {
        ///      "name" : "Arsenal",
        ///      "avatarURL": "https://commons.wikimedia.org/wiki/File:600px_Rosso_con_grifone_Bianco_scudato_e_fiamme.png"
        ///     }
        /// </remarks>        
        /// <returns>Returns newly created team</returns>
        /// <response code="200">Ok</response>      

        // POST: api/Teams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            _context.Teams.Add(team);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch( Exception ex)
            {

            }
            return CreatedAtAction("GetTeam", new { id = team.Id }, team);
        }

        /// <summary>
        /// Delete a team
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/Teams/6
        ///    
        /// </remarks>    
        /// <param name="id"></param>
        /// <response code="200">Team is removed</response>      
        /// <response code="404">If team is not found</response>    
        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(long id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool TeamExists(long id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
