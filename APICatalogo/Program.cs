using APICatalogo.ApiEndPoints;
using APICatalogo.AppServicesExtensions;
using APICatalogo.Context;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger()
    .AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJWT();


var app = builder.Build();

app.MapAutenticacaoEndPoints();
app.MapCategoriasEndPoints();
app.MapProdutosEndPoints();

var environment = app.Environment;
app.UseExcptionHandling(environment)
    .UseSwaggerMiddlewre()
    .UserAppCors();



app.UseAuthentication();
app.UseAuthorization();


app.Run();
