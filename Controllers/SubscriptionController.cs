using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SubscriptionController(BlogDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        // GET: Subscription
        public async Task<IActionResult> Index()
        {
            var blogDbContext = _context.Subscriptions.Include(s => s.Post).Include(s => s.User);
            return View(await blogDbContext.ToListAsync());
        }

        // POST: Subscription/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscription = await _context.Subscriptions.Where(x => x.PostId == id).ToListAsync();
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription[0]);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Post", new { Id = id });
        }

        // POST: Subscription/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,PostId")] Subscription subscription)
        {
            // hämtar post och user
            IdentityUser? user = await _userManager.GetUserAsync(HttpContext.User);
            subscription.UserId = user?.Id;
            subscription.User = await _context.Users.FindAsync(subscription.UserId);
            subscription.Post = await _context.Posts.FindAsync(subscription.PostId);

            // kollar om posten redan är prenumererad på
            var subscription1 = await _context.Subscriptions.Where(x => x.PostId == subscription.PostId).ToListAsync();
            if (subscription1.Count != 0)
            {  
                // skickar användaren tillbaka till posten de var på
                return RedirectToAction("Details", "Post", new { Id = subscription.PostId });
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Post", new { Id = subscription.PostId });
            }
            return View(subscription);
        }
    }
}
