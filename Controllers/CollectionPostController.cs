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
    public class CollectionPostController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CollectionPostController(BlogDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: CollectionPost/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
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

        private bool CollectionPostExists(int id)
        {
            return _context.CollectionPosts.Any(e => e.Id == id);
        }
    }
}
