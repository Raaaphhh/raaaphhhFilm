using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using raaaphhhFilm.Models;

namespace raaaphhhFilm.Controllers;

public class ContactController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IConfiguration configuration, ILogger<ContactController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    // GET
    public IActionResult Contact()
    {
        return View(new ContactFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(ContactFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var sender = _configuration["EmailSettings:Sender"]
                ?? throw new InvalidOperationException("EmailSettings:Sender non configuré");
            var password = _configuration["EmailSettings:Password"]
                ?? throw new InvalidOperationException("EmailSettings:Password non configuré");
            var receiver = _configuration["EmailSettings:Receiver"]
                ?? throw new InvalidOperationException("EmailSettings:Receiver non configuré");

            // Gmail n'autorise l'envoi que depuis le compte authentifié :
            // on envoie depuis Sender et on met le visiteur en Reply-To.
            var fromAddress = new MailAddress(sender, "raaaphhhFilm");
            var toAddress = new MailAddress(receiver);

            using var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, password)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = $"[raaaphhhFilm] Message de {model.Name}",
                Body = $"Nom: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}"
            };
            message.ReplyToList.Add(new MailAddress(model.Email, model.Name));

            smtp.Send(message);

            ModelState.Clear();
            ViewBag.Success = true;
            ViewBag.Message = "Message envoyé. Je te réponds vite !";
            return View(new ContactFormModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Échec de l'envoi du formulaire de contact");
            ViewBag.Success = false;
            ViewBag.Message = "L'envoi a échoué. Réessaie dans quelques minutes.";
            return View(model);
        }
    }
}
