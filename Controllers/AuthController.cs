using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using control.personal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace control.personal.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AuthController(IConfiguration configuration, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Método para generar el token basado en las credenciales recibidas
        /// </summary>
        /// <param name="credenciales"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync([FromBody] Credenciales credenciales)
        {
            Usuario user = await _userManager.FindByEmailAsync(credenciales.Email);
            var secretKey = _configuration.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);
            if (user != null)
            {

                var result = await _signInManager.CheckPasswordSignInAsync(user, credenciales.Password, false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var _tmpRoles = new List<Claim>();
                    _tmpRoles.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    _tmpRoles.Add(new Claim(ClaimTypes.Email, user.Email));
                    foreach (var rol in roles)
                    {
                        _tmpRoles.Add(new Claim(ClaimTypes.Role, rol));
                    }
                    ClaimsIdentity claimsId = new ClaimsIdentity(_tmpRoles);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = claimsId,
                        // Nuestro token va a durar un año
                        Expires = DateTime.UtcNow.AddYears(1),
                        // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                    return Content(tokenHandler.WriteToken(createdToken));
                }
            }
            return StatusCode(401);
        }
        public class Credenciales
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}