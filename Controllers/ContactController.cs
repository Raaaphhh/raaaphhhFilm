using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using raaaphhhFilm.Models;

namespace raaaphhhFilm.Controllers;

public class ContactController : Controller
{
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
                var fromAddress = new MailAddress($"{emailUser}", "Destinataire");
                var toAddress = new MailAddress("destinataire@example.com", "Destinataire");
                const string fromPassword = "TON_MDP"; // à sécuriser dans un fichier de config
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