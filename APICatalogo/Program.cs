using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDBContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

//definir os endpoints

app.MapGet("/", () => "Catálogo de produtos - 2025.").ExcludeFromDescription();


app.MapPost("/Categorias", async (Categoria categoria, AppDBContext db) =>
{
    db.Categorias?.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
}).WithName("Criar Categoria");

app.MapGet("/Categorias", async (AppDBContext db) => await db.Categorias.ToListAsync());

app.MapGet("/Categorias/{id:int}", async (int id, AppDBContext db) =>
{
    return await db.Categorias.FindAsync(id)
        is Categoria categoria ? Results.Ok(categoria) : Results.NotFound();
});

app.MapPut("/Categorias/{id:int}", async (int id, Categoria categoria, AppDBContext db) =>
{
    if (categoria.CategoriaId != id)
    {
        return Results.BadRequest();
    }

    var categoriaDB = await db.Categorias.FindAsync(id);
    if (categoriaDB is null)
    {
        return Results.NotFound();
    }

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok(categoria);
});

app.MapDelete("/Categorias/{id:int}", async (int id, AppDBContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);

    if (categoria is null)
    {
        return Results.NotFound();
    }

    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
