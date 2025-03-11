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

            Console.WriteLine(collectionPost.PostId);
            Console.WriteLine(collections.Count);
            var cPosts = await _context.CollectionPosts.ToListAsync();
            for (var i = 0; i < collections.Count; i++)
            {
                Console.WriteLine(collections[i].Id);
                Console.WriteLine(collections[i].Title);
                Console.WriteLine(collections[i].IsChecked);

                if (!collections[i].IsChecked)
                {
                    // om checkbox inte är checkad så tas CollectionPosten bort från tabellen
                    Console.WriteLine("är inte checked");
                    for (var u = 0; u < cPosts.Count; u++)
                    {
                        if (cPosts[u].CollectionId == collections[i].Id && cPosts[u].PostId == collectionPost.PostId)
                        {
                            Console.WriteLine("inlägg hittades U: " + collections[i].Id);
                            Console.WriteLine("cPosts[u].PostId U: " + cPosts[u].Id);

                            _context.CollectionPosts.Remove(cPosts[u]);
                            await _context.SaveChangesAsync();

                        }
                    }
                }
                else
                {
                    // FORSTÄTT MED ATT SKAPA COLLECTIONPOST
                    for (var index = 0; index < cPosts.Count; index++)
                    {
                        Console.WriteLine("cPosts[i].PostId: " + cPosts[index].PostId);
                        Console.WriteLine("collectionPost.PostId: " + collectionPost.PostId);
                        if (cPosts[i].PostId == collectionPost.PostId)
                        {
                            Console.WriteLine("inlägget hittades");
                            // return RedirectToAction("Details", "Post", new { Id = collectionPost.PostId });
                        }
                        else
                        {
                            Console.WriteLine("inlägget hittades inte");

                        }
                        Console.WriteLine();
                    }

                }
            }

            // await _context.SaveChangesAsync();



            // collectionPost.CollectionId = int.Parse(cId);
            // collectionPost.Post = await _context.Posts.FindAsync(collectionPost.PostId);

            // for (var i = 0; i < cPosts.Count; i++)
            // {
            //     if (cPosts[i].CollectionId == collectionPost.CollectionId && cPosts[i].PostId == collectionPost.PostId)
            //     {
            //         return RedirectToAction("Details", "Post", new { Id = collectionPost.PostId });
            //     }
            // }

            // if (ModelState.IsValid)
            // {
            //     _context.Add(collectionPost);
            //     await _context.SaveChangesAsync();
            //     return RedirectToAction("Details", "Post", new { Id = collectionPost.PostId });
            // }
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Title", collectionPost.CollectionId);
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
