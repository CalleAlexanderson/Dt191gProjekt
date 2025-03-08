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

        // GET: Subscription
        public async Task<IActionResult> Index()
        {
            var blogDbContext = _context.Subscriptions.Include(s => s.Post).Include(s => s.User);
            return View(await blogDbContext.ToListAsync());
        }

        // POST: Subscription/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine("funkar: " +id);
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Subscription/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,PostId")] Subscription subscription)
        {
            IdentityUser? user = await _userManager.GetUserAsync(HttpContext.User);
            subscription.UserId = user?.Id;
            subscription.User = await _context.Users.FindAsync(subscription.UserId);
            subscription.Post = await _context.Posts.FindAsync(subscription.PostId);

            var subscription1 = await _context.Subscriptions.Where(x => x.PostId == subscription.PostId).ToListAsync();
            Console.WriteLine("sub: "+subscription1);
            if (subscription1 != null)
            {  
                ViewData["subError"] = "Detta inlägg prenumereras redan på";
                // FIXA SÅ DEN HÄR JÄVLA REDIRECT GÅR RÄTT
                return RedirectToAction("Details/3", "Post");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction("Posts", "Post");
            }
            return View(subscription);
        }

        // GET: Subscription/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Post)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }
    }
}
