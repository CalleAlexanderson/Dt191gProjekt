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
    public class PostController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public PostController(BlogDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var blogDbContext = _context.Posts.Include(p => p.User);
            return View(await blogDbContext.ToListAsync());
        }

        // GET: Post/Posts
        public async Task<IActionResult> Posts()
        {
            var blogDbContext = _context.Posts.Include(p => p.User);
            return View(await blogDbContext.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // kollar om posten finns
            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            // kollar om posten skapades av den inloggade användaren
            IdentityUser? user = await _userManager.GetUserAsync(HttpContext.User);
            if (post.User == user)
            {
                ViewData["own"] = "true";
            } else {
                ViewData["own"] = "false";
            }

            // skickar med om post är prenumererad på
            var subscription = await _context.Subscriptions.Where(x => x.PostId == post.Id).ToListAsync();
            if (subscription.Count != 0)
            {
                ViewData["subId"] = subscription[0].Id;
                ViewData["sub"] = "true";
            } else {
                ViewData["sub"] = "false";
            }

            
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Title");

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Content")] Post post)
        {   
            // Hämtar user och lägger till på posten
            IdentityUser? user = await _userManager.GetUserAsync(HttpContext.User);
            post.UserId = user?.Id;
            post.User = await _context.Users.FindAsync(post.UserId);

            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Posts));
            }
            return View(post);
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // kollar om posten finns
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Content")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            // hämtar user (samma som create)
            IdentityUser? user = await _userManager.GetUserAsync(HttpContext.User);
            post.UserId = user?.Id;
            post.User = await _context.Users.FindAsync(post.UserId);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Posts));
            }
            return View(post);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Posts));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
