using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WareHouseERP.Authorization;

public static class AuthorizationEndpoints
{
    public static void MapAuthorizationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
                       .WithTags("Authorization");

        group.MapPost("/register", Register)
                .WithName("Register");
        group.MapPost("/login", Login)
                .WithName("Login");
        group.MapPost("/logout", Logout)
                .RequireAuthorization()
                .WithName("Logout");
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterRequest request,
        UserManager<IdentityUser> userManager)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { message = "Email and password are required" });
        }

        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        return Results.Created($"/api/auth/register", new { message = "User registered successfully", email = user.Email });
    }

    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { message = "Email and password are required" });
        }

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Results.Unauthorized();
        }

        var result = await signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, false);
        if (!result.Succeeded)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(new { message = "Login successful", email = user.Email });
    }

    private static async Task<IResult> Logout(
        SignInManager<IdentityUser> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok(new { message = "Logout successful" });
    }
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
