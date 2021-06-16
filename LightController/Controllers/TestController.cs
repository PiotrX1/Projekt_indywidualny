using LightController.Classes;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace LightController.Controllers
{
    public class TestController : Controller
    {
        private IEmailSender _emailSender;
        
        public TestController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        
        
        // GET
        public IActionResult Index()
        {

            _emailSender.SendMessage("piotr020798@outlook.com", "test", "Testowa wiadomość");
            
            //var emailSender = new EmailSender("p-kowalczyk@outlook.com", "wclebwpnlkbvsvzb", "smtp-mail.outlook.com", 587, SecureSocketOptions.Auto );


            //emailSender.SendMessage
            
            
            return View();
        }
    }
}