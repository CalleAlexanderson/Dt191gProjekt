using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class Collection {
    public int Id {get; set;}

    [Required(ErrorMessage = "Ange samlingens titel")]
    public string? Title {get; set;}

    public ICollection<Post>? Posts {get; set;}

    public IdentityUser? User {get; set;}

    [Required(ErrorMessage = "Ange UserId på den användare som skapar denna samling")]
    public string? UserId {get; set;}
}
