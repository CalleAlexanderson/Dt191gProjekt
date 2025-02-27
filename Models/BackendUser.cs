using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class BackendUser : IdentityUser
{

    [Required(ErrorMessage = "Ange ditt förnamn")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Ange ditt efternamn")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Ange ett lösenord")]
    [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken långt")]
    public string? Password {get; set;}
}