using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMF_Portal_API.IdentityAuth;
using EMF_Portal_API.Model;

namespace EMF_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisabilitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DisabilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Disabilities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Disability>>> GetDisability()
        {
            return await _context.Disability.ToListAsync();
        }

        // GET: api/Disabilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Disability>> GetDisability(int id)
        {
            var disability = await _context.Disability.FindAsync(id);

            if (disability == null)
            {
                return NotFound();
            }

            return disability;
        }

        // PUT: api/Disabilities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisability(int id, Disability disability)
        {
            if (id != disability.DisabilityID)
            {
                return BadRequest();
            }

            _context.Entry(disability).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisabilityExists(id))
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

        // POST: api/Disabilities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Disability>> PostDisability(Disability disability)
        {
            _context.Disability.Add(disability);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDisability", new { id = disability.DisabilityID }, disability);
        }

        // DELETE: api/Disabilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisability(int id)
        {
            var disability = await _context.Disability.FindAsync(id);
            if (disability == null)
            {
                return NotFound();
            }

            _context.Disability.Remove(disability);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DisabilityExists(int id)
        {
            return _context.Disability.Any(e => e.DisabilityID == id);
        }
    }
}
