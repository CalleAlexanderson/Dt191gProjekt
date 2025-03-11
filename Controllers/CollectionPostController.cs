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
    public class CollectionPostController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CollectionPostController(BlogDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CollectionPost
        public async Task<IActionResult> Index()
        {

            var blogDbContext = _context.CollectionPosts.Include(c => c.Collection);
            return View(await blogDbContext.ToListAsync());
        }

        // GET: CollectionPost/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectionPost = await _context.CollectionPosts
                .Include(c => c.Collection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collectionPost == null)
            {
                return NotFound();
            }

            return View(collectionPost);
        }

        // GET: CollectionPost/Create
        public IActionResult Create()
        {
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Title");
            return View();
        }

        // POST: CollectionPost/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostId")] CollectionPost collectionPost, List<Collection> collections)
        {

            // hämtar alla posts som ligger i collection redan
            var cPosts = await _context.CollectionPosts.ToListAsync();

            // håller koll på om en post redan finns i en collection
            bool notFoundInDb = true;

            // Collectionposts som ska läggas till i databasen
            List<CollectionPost> colPosts = [];

            for (var i = 0; i < collections.Count; i++)
            {
                // kollar om checkboxen på sidan är checked
                if (!collections[i].IsChecked)
                {
                    // om checkbox inte är checkad så tas CollectionPosten bort från tabellen
                    for (var u = 0; u < cPosts.Count; u++)
                    {
                        // kollar om collectionposten matchar i databasen
                        if (cPosts[u].CollectionId == collections[i].Id && cPosts[u].PostId == collectionPost.PostId)
                        {
                            _context.CollectionPosts.Remove(cPosts[u]);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    
                    for (var u = 0; u < cPosts.Count; u++)
                    {
                        // kollar om collectionposten redan finns i databasen
                        if (cPosts[u].CollectionId == collections[i].Id && cPosts[u].PostId == collectionPost.PostId)
                        {
                            notFoundInDb = false;
                        }
                    }
                    // om collectionposten inte finns kollas modelstate och den nuvarande posten läggs till i colPosts
                    if (notFoundInDb)
                    {
                        if (ModelState.IsValid)
                        {
                            collectionPost.CollectionId = collections[i].Id;

                            // skapar en ny CollectionPost som sedan läggs i colPosts
                            colPosts.Add(new CollectionPost { PostId = collectionPost.PostId, CollectionId = collectionPost.CollectionId });
                        }
                    }
                    else
                    {
                        notFoundInDb = true;
                    }

                }
            }

            // lägger till hela colPosts i CollectionPosts tabellen
            _context.CollectionPosts.AddRange(colPosts);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Post", new { Id = collectionPost.PostId });
        }

        // GET: CollectionPost/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectionPost = await _context.CollectionPosts.FindAsync(id);
            if (collectionPost == null)
            {
                return NotFound();
            }
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Title", collectionPost.CollectionId);
            return View(collectionPost);
        }

        // POST: CollectionPost/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostsId,CollectionId")] CollectionPost collectionPost)
        {
            if (id != collectionPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collectionPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionPostExists(collectionPost.Id))
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
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Title", collectionPost.CollectionId);
            return View(collectionPost);
        }

        // GET: CollectionPost/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectionPost = await _context.CollectionPosts
                .Include(c => c.Collection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collectionPost == null)
            {
                return NotFound();
            }

            return View(collectionPost);
        }

        // POST: CollectionPost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collectionPost = await _context.CollectionPosts.FindAsync(id);
            if (collectionPost != null)
            {
                _context.CollectionPosts.Remove(collectionPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectionPostExists(int id)
        {
            return _context.CollectionPosts.Any(e => e.Id == id);
        }
    }
}
