using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using control.personal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using control.personal.Services;
using System.Text.RegularExpressions;
using control.personal.Data;

namespace control.personal.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly TelegramService _telegramService;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            TelegramService telegramService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _configuration = configuration;
            _telegramService = telegramService;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "El {0} deberia tener al menos {2} y maximo {1} caracteres", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "El password y la confirmacion no coinciden")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(10, ErrorMessage = "La cédula consta de 10 caracteres", MinimumLength = 6)]
            [Display(Name = "Cedula de Identidad")]
            public string Cedula { get; set; }

            [Required]
            [Display(Name = "Nombre Completo")]
            public string Nombre { get; set; }
        }

        public async Task OnGetAsync( string returnUrl = null)
        {
            //var telegram = await _telegramService.SendMessage("https://telegrambots.github.io/book/2/send-msg/other-msg.html");
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            #region validacionCedula
            //si existe un usuario con dicha cedula procedemos a redirigir a la misma pagina pero con un error
            //esto puede hacerse con un custom validator https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0
            if (_context.Users.Any(x => String.Equals(x.Cedula, Input.Cedula)))
            {
                ModelState.AddModelError(string.Empty, "Cedula ya utilizada");
                return Page();
            }
            #endregion
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Usuario { UserName = Input.Email, Email = Input.Email, Cedula = Input.Cedula, Nombre = Input.Nombre };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    //asignación de rol por defecto
                    if (String.Equals(user.Email, _configuration["Administracion:General"]))
                    {

                        await _userManager.AddToRoleAsync(user, "Administrador");
                        _logger.LogInformation("Administrador registrado");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Empleado");
                        _logger.LogInformation("Empleado registrado");

                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //este callback debe ser enviado para la verificación por parte del administrador
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    var telegram = await _telegramService.SendMessage(
                                    _telegramService.UrlBodyFormateado(callbackUrl, $"Verifique la cuenta de {Input.Nombre} con el siguiente enlace", "Enlace")
                                    );
                    string mensaje = "Registro no enviado a telegram";
                    if (String.Equals(telegram.ToLower(), "ok"))
                    {
                        mensaje = "Registro enviado a telegram";
                        _logger.LogInformation(mensaje);
                        return Redirect("~/");
                    }
                    else
                    {
                        //si por algun motivo no se puede enviar el enlace a telegram se puede usar la pagina para el registro por defecto
                        _logger.LogInformation(mensaje);
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
