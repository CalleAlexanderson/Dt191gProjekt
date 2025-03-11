using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class Post {
    public int Id {get; set;}

    [Required(ErrorMessage = "Ange blogginläggets titel")]
    [MaxLength(30)]
    public string? Title {get; set;}

    [MaxLength(200)]
    public string? Description {get; set;}

    public string? Content {get; set;}

    public DateOnly Date {get; set;} = DateOnly.FromDateTime(DateTime.Now);

    public IdentityUser? User {get; set;}
    
    public string? UserId {get; set;}

    [NotMapped]
    public List<Collection>? Collections {get; set;}
}