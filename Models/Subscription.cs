using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Subscription {
    public int Id {get; set;}
    public User? User {get; set;}

    [Required(ErrorMessage = "Ange UserId på den användare som ska premunerera på detta blogginlägg")]
    public int UserId {get; set;}

    public Post? Post {get; set;}

    [Required(ErrorMessage = "Ange PostId på det blogginlägg som ska premuneras på")]
    public int PostId {get; set;}
}