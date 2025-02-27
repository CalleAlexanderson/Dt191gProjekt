using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data;

public class BlogDbContext : DbContext {
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) 
    {}

    public DbSet<User> Users {get; set;}

    public DbSet<Post> Posts {get; set;}

    public DbSet<Collection> Collections {get; set;}

    public DbSet<Subscription> Subscriptions {get; set;}
}