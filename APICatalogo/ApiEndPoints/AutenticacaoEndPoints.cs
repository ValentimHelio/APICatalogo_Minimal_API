using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;

namespace APICatalogo.ApiEndPoints;

public static class AutenticacaoEndPoints
{
    public static void MapAutenticacaoEndPoints(this WebApplication app)
    {
        //endpoint para login
        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if (userModel == null)
            {
                return Results.BadRequest("Login Invalido");
            }
            if (userModel.UserName == "macoratti" && userModel.Password == "numsey#123")
            {
                var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"], app.Configuration["Jwt:Issuer"], app.Configuration["Jwt:Audience"], userModel);
                return Results.Ok(new { token = tokenString });
            }
            else
            {
                return Results.BadRequest("Login Invalido");
            }
        }).Produces(StatusCodes.Status400BadRequest)
                      .Produces(StatusCodes.Status200OK)
                      .WithName("Login")
                      .WithTags("Autenticacao");
    }
}
