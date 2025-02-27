using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data;

public class BlogDbContext : IdentityDbContext {
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) 
    {}

    public DbSet<Post> Posts {get; set;}

    public DbSet<Collection> Collections {get; set;}

    public DbSet<Subscription> Subscriptions {get; set;}
}