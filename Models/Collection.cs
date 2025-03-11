using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class Collection {
    public int Id {get; set;}

    [Required(ErrorMessage = "Ange samlingens titel")]
    public string? Title {get; set;}

    public IdentityUser? User {get; set;}
    public string? UserId {get; set;}

    [NotMapped]
    public bool IsChecked {get; set;}
}
