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
    public class CollectionController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CollectionController(BlogDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Collection
        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<Collection> collections = await _context.Collections.ToListAsync();
            List<int> collectionSizes = [];

            // skapar en lista med mängden inlägg i de olika samlingarna användaren skapat
            for (var i = 0; i < collections.Count; i++)
            {
                List<CollectionPost> collectionPosts = await _context.CollectionPosts.Where(t => t.CollectionId == collections[i].Id).ToListAsync();
                collectionSizes.Add(collectionPosts.Count);
            }
            ViewData["collectionSizes"] = collectionSizes.ToArray();
            
            var blogDbContext = _context.Collections.Include(c => c.User);
            return View(await blogDbContext.ToListAsync());
        }

        // GET: Collection/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var blogDbContext = _context.Collections.Include(c => c.User);
            var posts = await _context.Posts.ToListAsync();
            var users = await _context.Users.ToListAsync();
            List<CollectionPost> collectionPosts = await _context.CollectionPosts.Where(t => t.CollectionId == id).ToListAsync();
            
            var collection = await _context.Collections
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            collection.Posts = collectionPosts;

            return View(collection);
        }

         // POST: Collection/Details/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCollectionPosts([Bind("Id,Title")] Collection collection, List<CollectionPost> posts)
        {
            // tar bort de inlägg som checkats 
            for (var i = 0; i < posts.Count; i++)
            {
                if (posts[i].IsChecked)
                {
                    _context.CollectionPosts.Remove(posts[i]);
                }
            }

            await _context.SaveChangesAsync(); 
            
            return RedirectToAction("Details", new { id = collection.Id });
        }

        // GET: Collection/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Collection/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Collection collection)
        {
            // Hämtar user och lägger till på samlingen
            IdentityUser? user = await _userManager.GetUserAsync(HttpContext.User);
            collection.UserId = user?.Id;
            collection.User = await _context.Users.FindAsync(collection.UserId);
            if (ModelState.IsValid)
            {
                _context.Add(collection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", collection.UserId);
            return View(collection);
        }

        // GET: Collection/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", collection.UserId);
            return View(collection);
        }

        // POST: Collection/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Collection collection)
        {
            if (id != collection.Id)
            {
                return NotFound();
            }

            // Hämtar user och lägger till på samlingen
            IdentityUser? user = await _userManager.GetUserAsync(HttpContext.User);
            collection.UserId = user?.Id;
            collection.User = await _context.Users.FindAsync(collection.UserId);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionExists(collection.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", collection.UserId);
            return View(collection);
        }

        // GET: Collection/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        // POST: Collection/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collection = await _context.Collections.FindAsync(id);
            if (collection != null)
            {
                _context.Collections.Remove(collection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectionExists(int id)
        {
            return _context.Collections.Any(e => e.Id == id);
        }
    }
}
