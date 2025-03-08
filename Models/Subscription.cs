using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class Subscription {
    public int Id {get; set;}
    public IdentityUser? User {get; set;}

    public string? UserId {get; set;}

    public Post? Post {get; set;}

    public int PostId {get; set;}
}