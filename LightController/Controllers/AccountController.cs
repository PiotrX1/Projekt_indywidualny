using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LightController.Classes;
using LightController.Data;
using LightController.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LightController.Controllers
{
    public class AccountController : Controller
    {

        private AppDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private IEmailSender _emailSender;
        
        public AccountController(AppDbContext dbContext, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        
        
        public IActionResult Index()
        {

            _dbContext.Functions.Add(new Function()
            {
                Driver = null,
                Name = "dsfdsf"
            });

            _dbContext.SaveChanges();
            
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var registerModel = new RegisterModel();
            
            return View(registerModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<ApplicationUser>();

                if ((await _userManager.FindByNameAsync(registerModel.Username)) == null &&
                    (await _userManager.FindByEmailAsync(registerModel.Email)) == null)
                {

                    var user = new ApplicationUser()
                    {
                        UserName = registerModel.Username,
                        NormalizedUserName = registerModel.Username.ToUpper(),
                        Email = registerModel.Email,
                        PasswordHash = hasher.HashPassword(null, registerModel.Password),
                        EmailConfirmed = false
                    };
                    
                    
                    
                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", code);
                        //await _userManager.SetEmailAsync(user, "piotr020798@outlook.com");
                        
                        //_emailSender.SendMessage("piotr020798@outlook.com", "Aktywacja konta", callbackUrl);
                        
                        

                    }
                    
                }
                else
                {
                    ModelState.AddModelError("EmailOrUsernameAlreadyInUse", "Adres email lub login jest już zajęty");
                }

            }
            
            return View(registerModel);
        }
        
        
        
        public IActionResult ConfirmEmail()
        {
            return View();
        }
        
    }
}