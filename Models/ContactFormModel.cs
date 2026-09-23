using System.ComponentModel.DataAnnotations;

namespace raaaphhhFilm.Models;

public class ContactFormModel
{
    [Required(ErrorMessage = "Indique ton nom.")]
    [StringLength(100, ErrorMessage = "100 caractères maximum.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Indique ton e-mail pour que je puisse te répondre.")]
    [EmailAddress(ErrorMessage = "Cette adresse e-mail n'est pas valide.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Écris ton message.")]
    [StringLength(5000, ErrorMessage = "5000 caractères maximum.")]
    public string Message { get; set; } = string.Empty;
}
