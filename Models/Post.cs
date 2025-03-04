using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class Post {
    public int Id {get; set;}

    [Required(ErrorMessage = "Ange blogginläggets titel")]
    public string? Title {get; set;}

    public string? Description {get; set;}

    public string? Content {get; set;}

    public DateOnly Date {get; set;} = DateOnly.FromDateTime(DateTime.Now);

    public IdentityUser? User {get; set;}
    
    public string? UserId {get; set;}
}