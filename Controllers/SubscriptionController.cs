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
    public class SubscriptionController : ControllerBase
    {
        private readonly BlogDbContext _context;

        public SubscriptionController(BlogDbContext context)
        {
            _context = context;
        }

        // GET: api/Subscription
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions()
        {
            var users = await _context.Users.ToListAsync();
            var posts = await _context.Posts.ToListAsync();
            return await _context.Subscriptions.ToListAsync();
        }

        // GET: api/Subscription/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
            var users = await _context.Users.ToListAsync();
            var posts = await _context.Posts.ToListAsync();
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // PUT: api/Subscription/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscription(int id, Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return BadRequest();
            }

            // Hämtar en användare och lägger den i post.User
            var user = await _context.Users.FindAsync(subscription.UserId);
            // validering om man inte går från frontend
            if (user == null)
            {
                return BadRequest("Användaren hittas inte i databasen");
            }
            subscription.User = user;

            var post = await _context.Posts.FindAsync(subscription.PostId);
            // validering om man inte går från frontend
            if (post == null)
            {
                return BadRequest("Blogginlägget hittas inte i databasen");
            }
            subscription.Post = post;

            _context.Entry(subscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
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

        // POST: api/Subscription
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
        {
            // Hämtar en användare och lägger den i post.User
            var user = await _context.Users.FindAsync(subscription.UserId);
            // validering om man inte går från frontend
            if (user == null)
            {
                return BadRequest("Användaren hittas inte i databasen");
            }
            subscription.User = user;

            var post = await _context.Posts.FindAsync(subscription.PostId);
            // validering om man inte går från frontend
            if (post == null)
            {
                return BadRequest("Blogginlägget hittas inte i databasen");
            }
            subscription.Post = post;

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscription", new { id = subscription.Id }, subscription);
        }

        // DELETE: api/Subscription/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscriptions.Any(e => e.Id == id);
        }
    }
}
