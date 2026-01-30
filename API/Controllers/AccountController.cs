using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Security.Claims;
using Core.Entities;
using Core.Interfaces;
using Infrastucture.Services.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class RegisterRequest
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}

public class PreRegistrationInfo
{
    public string Password { get; set; }
    public string Code { get; set; }
}

public class AccountController(SignInManager<AppUser> signInManager, 
                               UserManager<AppUser> userManager, 
                               ICacheService cacheService,
                               IEmailService emailService) : BaseApiController
{
    [HttpPost("pre-register")]
    public async Task<ActionResult> PreRegister(RegisterRequest request, [FromServices] IPasswordHasher<AppUser> passwordHasher)
    {
        if (await userManager.FindByEmailAsync(request.Email) != null)
            return BadRequest("User already exists");
        
        var dummyUser = new AppUser { Email = request.Email };
        string hashedByApp = passwordHasher.HashPassword(dummyUser, request.Password);
        
        var cacheKey = $"verify_{request.Email}";
        var verificationCode = new Random().Next(100000, 999999).ToString();
        var duration = new TimeSpan(0, 5, 0);
        
        await cacheService.SetCacheAsync<PreRegistrationInfo>(
            cacheKey, 
            new() { Password = hashedByApp, Code = verificationCode }, 
            duration);
        
        var emailBody = EmailTemplates.GetOtpTemplate(verificationCode);
        await emailService.SendEmailAsync(request.Email, "Verify your email", emailBody);
        
        return Ok("Verification code sent to email.");
    }

    [HttpPost("verify-email")]
    public async Task<ActionResult> Verify(string email, string code)
    {
        if (await userManager.FindByEmailAsync(email) != null)
            return BadRequest("User already exists");
        
        var cacheKey = $"verify_{email}";
        
        if (!await cacheService.IsExistAsync(cacheKey))
            return BadRequest("Something went wrong with code verification");
        
        var cachedData = await cacheService.GetCacheAsync<PreRegistrationInfo>(cacheKey);
        
        if (cachedData.Code != code)
            return BadRequest("Code is wrong");
        
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            PasswordHash = cachedData.Password
        };
        
        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            
            return ValidationProblem();
        }
        
        await cacheService.RemoveCacheAsync(cacheKey);
        
        var emailBody = EmailTemplates.GetWelcomeTemplate();
        await emailService.SendEmailAsync(email, "Successfully registered to Cinemax!", emailBody);

        return Ok();
    }
    
    [Authorize]
    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false) return NoContent();

        var user = await signInManager.UserManager.GetUserByEmail(User);
        
        return Ok(new
        {
            user.FullName,
            user.Email
        });
    }
}

public static class ClaimsExtensions
{
    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager,
                                                     ClaimsPrincipal user)
    {
        var userToReturn = await userManager.Users
           .FirstOrDefaultAsync(x => x.Email == user.GetEmail());
        
        if (userToReturn == null) throw new AuthenticationException("User not found");
        
        return userToReturn;
    }
    
    public static string GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email) 
            ?? throw new AuthenticationException("Email claim doesn't exist");
        
        return email;
    }
}