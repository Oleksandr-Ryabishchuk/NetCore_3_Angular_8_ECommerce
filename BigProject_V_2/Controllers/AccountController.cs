using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BigProject_V_2.BusinessLayer.Interfaces;
using BigProject_V_2.DataAccessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BigProject_V_2.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationSettings _applicationSettings;
        private readonly IEmailManager _emailManager;
        public AccountController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, 
            IOptions<ApplicationSettings> applicationSettings, 
            IEmailManager emailManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationSettings = applicationSettings.Value;
            _emailManager = emailManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            List<string> errorList = new List<string>();

            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callBackUrl = Url.Action("ConfirmEmail", "Account", new { UserId = user.Id, Code = code }, protocol: HttpContext.Request.Scheme);

                await _emailManager.SendEmailAsync(user.Email, "SpicyCo.com - Підтвердження реєстрації", 
                    "Будь ласка підтвердіть вашу електронну адресу за посиланням: <a href=\"" + callBackUrl + "\">натисніть тут</a>");

                return Ok(new { username = user.UserName,   phoneNumber = user.PhoneNumber, 
                    email = user.Email, status = 1, message = "Registration Successfull" });
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorList.Add(error.Description);
                }
            }
            return BadRequest(new JsonResult(errorList));            
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_applicationSettings.Secret));

            double tokenExpiryTime = Convert.ToDouble(_applicationSettings.ExpireTime);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if(! await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Користувач не підтведив електронну адресу");
                    return Unauthorized(new { LoginError = 
                        "Ми відправили Вам підтвердження реєстрації на електронну адресую Будь ласка підтвердіть вашу реєстрацію на SpicyCo.com" });                    
                }
                var roles = await _userManager.GetRolesAsync(user);

                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim("LoggedOn", DateTime.Now.ToString())
                    }),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                    Issuer = _applicationSettings.Site,
                    Audience = _applicationSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new { token = tokenHandler.WriteToken(token), expiration = token.ValidTo, email = user.Email, userName = user.UserName, userRole = roles.FirstOrDefault() });
            }
            ModelState.AddModelError("", "Ім'я користувача або пароль не знайдено");
            return Unauthorized(new { LoginError = "Будь ласка, перевірте вказані дані" });
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if( string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return new JsonResult("Error");
            }
            if (user.EmailConfirmed)
            {
                return Redirect("/login");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return RedirectToAction("EmailConfirmed", "Notifications", new { userId, code });
            }
            else
            {
                List<string> errors = new List<string>();
                foreach(var error in result.Errors)
                {
                    errors.Add(error.ToString());
                }
                return new JsonResult(errors);
            }
        }
    }
}