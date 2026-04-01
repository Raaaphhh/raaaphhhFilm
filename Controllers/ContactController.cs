using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using raaaphhhFilm.Models;

namespace raaaphhhFilm.Controllers;

public class ContactController : Controller
{
    private readonly IConfiguration _configuration;

    public ContactController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // GET
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Contact(ContactFormModel model, string emailUser)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var fromPassword = _configuration["EmailSettings:Password"]
                    ?? throw new InvalidOperationException("EmailSettings:Password non configuré");
                var receiver = _configuration["EmailSettings:Receiver"]
                    ?? throw new InvalidOperationException("EmailSettings:Receiver non configuré");
                var fromAddress = new MailAddress($"{emailUser}", "Destinataire");
                var toAddress = new MailAddress(receiver, "Destinataire");
                string subject = $"Message de {model.Name}";
                string body = $"Nom: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // à adapter selon ton fournisseur
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };

                smtp.Send(message);
                ViewBag.Message = "Message envoyé avec succès !";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erreur lors de l'envoi : {ex.Message}";
            }
        }

        return View(model);
    }
}