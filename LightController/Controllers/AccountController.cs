using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LightController.Classes;
using LightController.Data;
using LightController.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LightController.Controllers
{
    public class AccountController : Controller
    {

        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(AppDbContext dbContext, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._configuration = configuration;
            this._signInManager = signInManager;
        }
        
        
        public IActionResult Index()
        {


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
                        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        
                        _dbContext.Users.First(x => x.UserName == user.UserName).ActivationToken = token;

                        await _dbContext.SaveChangesAsync();
                        
                        var url = Url.Action("ConfirmEmail", "Account", new {token});
                        

                        string mailBody = "Link aktywacyjny: " + _configuration["applicationUrl"] + url;


                        _emailSender.SendMessage(user.Email, "Aktywacja konta", mailBody);


                        registerModel.Info = "Wysłaliśmy wiadomość z linkiem aktywacyjnym na adres " + user.Email;


                    }
                    
                }
                else
                {
                    ModelState.AddModelError("EmailOrUsernameAlreadyInUse", "Adres email lub login jest już zajęty");
                }

            }
            
            return View(registerModel);
        }
        
        
        
        public async Task<IActionResult> ConfirmEmail(string token = "")
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.ActivationToken == token && x.EmailConfirmed == false);
            
            
            if (user != null)
            {
                user.EmailConfirmed = true;
                await _dbContext.SaveChangesAsync();
                ViewBag.Info = "Konto aktywowane. Możesz się zalogować.";
            }
            else
            {
                ViewBag.Info = "Wystąpił błąd.";
            }
            
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Niepoprawne dane logowania.");
                }
                else
                {

                    if (user.EmailConfirmed == false)
                    {
                        ModelState.AddModelError("message", "Musisz najpierw aktywować konto");
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if(result.IsLockedOut)
                        {
                            ModelState.AddModelError("message", "Konto jest zablokowane");
                        }
                    }
                    
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public IActionResult PasswordReset()
        {
            
            return View();
        }
        
        
        [HttpPost]
        public async Task<IActionResult> PasswordReset(PasswordResetModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordReset= new PasswordResetToken()
                    {
                        User = user,
                        Token = token,
                        ExpirationTime = (DateTime.Now).AddMinutes(30),
                        Used = false
                    };

                    _dbContext.PasswordResetTokens.Add(passwordReset);

                    await _dbContext.SaveChangesAsync();
                    
                    
                    var url = Url.Action("PasswordReset2", "Account", new {token});


                    var mailBody = "Link do zmiany hasła: " + _configuration["applicationUrl"] + url;
                    
                    
                    _emailSender.SendMessage(user.Email, "Reset hasła", mailBody);



                    ViewBag.Info = "Na podany adres został wysłany email z linkiem do zmiany hasła.";

                }
                else
                {
                    ModelState.AddModelError("info", "Użytkownik o podanym adresie email nie istnieje");
                }
            }
            
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> PasswordReset2(string token = "")
        {
            
            var passwordResetToken = await _dbContext.PasswordResetTokens.FirstOrDefaultAsync(x => x.Token == token);


            if (passwordResetToken != null)
            {
      
                if (passwordResetToken.ExpirationTime <= DateTime.Now)
                {
                    ViewBag.Error = "Token wygasł";
                }
                else if (passwordResetToken.Used == true)
                {
                    ViewBag.Error = "Token został już wykorzystany";
                }
            }
            else
            {
                ViewBag.Error = "Token jest nieprawidłowy";
            }

            
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset2(PasswordReset2Model model, string token = "")
        {

            if (ModelState.IsValid)
            {
                var passwordResetToken = await _dbContext.PasswordResetTokens.Include(x=>x.User).FirstOrDefaultAsync(x => x.Token == token);
                
                if (passwordResetToken != null && passwordResetToken.ExpirationTime > DateTime.Now && passwordResetToken.Used == false)
                {

                    var hasher = new PasswordHasher<ApplicationUser>();

                    
                    passwordResetToken.User.PasswordHash = hasher.HashPassword(passwordResetToken.User, model.Password);
                    passwordResetToken.Used = true;
                    
                    
                    await _dbContext.SaveChangesAsync();

                    ViewBag.Info = "Hasło zmienione.";
 
                    
                }
                else
                {
                    return RedirectToAction("PasswordReset");
                }

                
                
            }


            return View();
        }



    }
}