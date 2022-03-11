using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EMF_Portal_API.IdentityAuth;
using NETCore.MailKit.Core;
using Microsoft.AspNetCore.WebUtilities;
using EMF_Portal_API.Extentions;
using Microsoft.AspNetCore.Authorization;
using EMF_Portal_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMF_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManagement;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailsevice;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        SendMails sendmails = new SendMails();
        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, IEmailService emailService, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, 
            ILogger<AuthenticationController> logger)
        {
            _userManagement = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailsevice = emailService;
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
    }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Register Regmodel, string userRole, string userid)
        {

            string Password = new PasswordGenerator().Generate(8, 9);
            Regmodel.Password = Password;
            

            var UserExists = await _userManagement.FindByNameAsync(Regmodel.UserName);
            
            if (UserExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Responce { Status = "Error", Message = "User already exist:" });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = Regmodel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = Regmodel.UserName,
                
            };
            await _context.SaveChangesAsync();

                var results = await _userManagement.CreateAsync(user, Regmodel.Password);
            
                if (results.Succeeded)
                {
                    var code = await _userManagement.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

                    await sendmails.SendConfirmationAsync(Regmodel.Email, callbackUrl, Regmodel.Password);
                    await _userManagement.AddToRoleAsync(user, userRole);
                
                if (userRole.Equals("Admin")) //|| userRole.Equals("Manager")
                {
                    var usertoupdate = await _context.UserRoles.SingleOrDefaultAsync(s => s.UserId == userid);
                    if (usertoupdate != null)
                    {
                        usertoupdate.UserId = user.Email;//it skips this find out
                        await _context.SaveChangesAsync();
                    }
                }
                _logger.LogInformation("User created a new account with password.");
                return Ok(new Responce { Status = "Success", Message = "User Created Successfully!" });
                }
            else 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Responce { Status = "Error", Message = "User creation failed! Please check details:" });
            }
        }

        //verfiy send
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userid, string code)
        {
            if (userid == null || code == null)
            {
                return RedirectToAction(nameof(AuthenticationController));
            }
            var user = await _userManagement.FindByIdAsync(userid);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userid}'.");
            }
            var result = await _userManagement.ConfirmEmailAsync(user, code);
           
            return Ok(new Responce { Status = "Success", Message = "Thank you for verifying your email, and please login!" });
                
        }
        
        [HttpPost]
        [Route("ForgetPassword")]

        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel forgetPassword)
        {
            var user = await _userManagement.FindByEmailAsync(forgetPassword.Email);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Responce { Status = "Error", Message = "User does not exist!" });
            }

            var codetoken = await _userManagement.GeneratePasswordResetTokenAsync(user);
            var urlcallback = Url.ResetPasswordCallbackLink(user.Id, codetoken, Request.Scheme);
            await sendmails.PasswordResetAsync(forgetPassword.Email, urlcallback);

            return Ok(new Responce { Status = "Success", Message = "Password rest sent to your mail" });
        }

        [HttpPost]
        [Route("ResetPassword")]

        public async Task<IActionResult> ResetPassword(ForgetPasswordModel model)
        {
            var user = await _userManagement.FindByEmailAsync(model.Email);
            var result = await _userManagement.ResetPasswordAsync(user, model.Code, model.Password);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Responce { Status = "Error", Message = "Rest Password failed!" });
            }
            else
            {
                return Ok(new Responce { Status = "Success", Message = "Password rest successfuly!" });
            }
        }
        //[HttpPost]
        //[Route("LogIn")]
        //public async Task<IActionResult> LogIn([FromBody] Login logInmodel)
        //{
        //    var user = await _userManagement.FindByEmailAsync(logInmodel.Email);

        //    if (user != null && !user.EmailConfirmed)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Responce { Status = "Error", Message = "Email not confirmed!" });
        //    }
        //    if (await _userManagement.CheckPasswordAsync(user, logInmodel.Password) == false)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Responce { Status = "Error", Message = "Invalid Credentials!" });
        //    }

        //    var results = await signInManager.PasswordSignInAsync(logInmodel.Email, logInmodel.Password, logInmodel.RememberMe, true);
        //    if (results.Succeeded)
        //    {
        //        await _userManagement.AddClaimAsync(user, new Claim("UserRole", "Admin"));
        //        return
        //    }
        //    else if (results.IsLockedOut)
        //    {
        //        return
        //    }
        //    else
        //    {
        //        return
        //    }
        //    return
        //}

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] Login logInmodel)
        {
            var user = await _userManagement.FindByEmailAsync(logInmodel.Email);

            var result = await _signInManager.PasswordSignInAsync(logInmodel.Email, logInmodel.Password, logInmodel.RememberMe,true);

            if (user != null || result.Succeeded) //await _userManagement.CheckPasswordAsync(user, logInmodel.Password)
            {
                var userRoles = await _userManagement.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
               
                await _userManagement.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.Email));

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken
                (
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   expires: DateTime.Now.AddHours(3),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                );
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
            }

            if (user != null )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Responce { Status = "Error", Message = "Email not confirmed!" });
            }
            if (await _userManagement.CheckPasswordAsync(user, logInmodel.Password) == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Responce { Status = "Error", Message = "Invalid Credentials!" });
            }
            return Unauthorized();
        }
        //logout
        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new Responce { Status = "Success", Message = "Logout successfuly!" });
        }
    }
}
