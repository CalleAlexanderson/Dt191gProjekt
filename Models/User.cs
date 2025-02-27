using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ange ditt förnamn")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Ange ditt efternamn")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Ange din e-postadress")]
    [EmailAddress(ErrorMessage ="Ange en korrekt e-postadress")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Ange ett lösenord")]
    [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken långt")]
    public string? Password {get; set;}
}