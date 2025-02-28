using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly BlogDbContext _context;

        public CollectionController(BlogDbContext context)
        {
            _context = context;
        }

        // GET: api/Collection
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collection>>> GetCollections()
        {
            var users = await _context.Users.ToListAsync();
            var posts = await _context.Posts.ToListAsync();
            return await _context.Collections.ToListAsync();
        }

        // GET: api/Collection/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Collection>> GetCollection(int id)
        {
            var collection = await _context.Collections.FindAsync(id);

            if (collection == null)
            {
                return NotFound();
            }

            return collection;
        }

        // PUT: api/Collection/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollection(int id, Collection collection)
        {
            if (id != collection.Id)
            {
                return BadRequest();
            }

            _context.Entry(collection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectionExists(id))
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

        // POST: api/Collection
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Collection>> PostCollection(Collection collection)
        {
            // Hämtar en användare och lägger den i post.User
            var user = await _context.Users.FindAsync(collection.UserId);
            // validering om man inte går från frontend
            if (user == null)
            {
                return BadRequest("Användaren hittas inte i databasen");
            }

            collection.User = user;

            var posts = await _context.Posts.ToListAsync();

            collection.Posts = posts;
            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCollection", new { id = collection.Id }, collection);
        }

        // DELETE: api/Collection/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CollectionExists(int id)
        {
            return _context.Collections.Any(e => e.Id == id);
        }
    }
}
