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
    public class TitlesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TitlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Titles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Title>>> GetTitle()
        {
            return await _context.Title.ToListAsync();
        }

        // GET: api/Titles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Title>> GetTitle(int id)
        {
            var title = await _context.Title.FindAsync(id);

            if (title == null)
            {
                return NotFound();
            }

            return title;
        }

        // PUT: api/Titles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTitle(int id, Title title)
        {
            if (id != title.TitleID)
            {
                return BadRequest();
            }

            _context.Entry(title).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TitleExists(id))
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

        // POST: api/Titles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Title>> PostTitle(Title title)
        {
            _context.Title.Add(title);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTitle", new { id = title.TitleID }, title);
        }

        // DELETE: api/Titles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTitle(int id)
        {
            var title = await _context.Title.FindAsync(id);
            if (title == null)
            {
                return NotFound();
            }

            _context.Title.Remove(title);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TitleExists(int id)
        {
            return _context.Title.Any(e => e.TitleID == id);
        }
    }
}
