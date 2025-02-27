using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Collection {
    public int Id {get; set;}
    public Post[]? Posts {get; set;}

    public BackendUser? User {get; set;}

    [Required(ErrorMessage = "Ange UserId på den användare som skapar denna samling")]
    public int UserId1 {get; set;}
}
