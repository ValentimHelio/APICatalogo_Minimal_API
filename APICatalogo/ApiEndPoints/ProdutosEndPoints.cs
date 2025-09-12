using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.ApiEndPoints;

public static class ProdutosEndPoints
{
    public static void MapProdutosEndPoints(this WebApplication app)
    {
        app.MapPost("/Produtos", async (Produto produto, AppDBContext db) =>
        {
            db.Produtos?.Add(produto);
            await db.SaveChangesAsync();

            return Results.Created($"/produtos/{produto.ProdutoId}", produto);
        }).WithName("Criar Produto");

        app.MapGet("/Produto", async (AppDBContext db) => await db.Produtos.ToListAsync()).WithTags("Produto").RequireAuthorization();

        app.MapGet("/Pategorias/{id:int}", async (int id, AppDBContext db) =>
        {
            return await db.Produtos.FindAsync(id) is Produto produto ? Results.Ok(produto) : Results.NotFound();
        });

        app.MapPut("/Produto/{id:int}", async (int id, Produto produto, AppDBContext db) =>
        {
            if (produto.ProdutoId != id)
            {
                return Results.BadRequest();
            }

            var produtoDB = await db.Produtos.FindAsync(id);
            if (produtoDB is null)
            {
                return Results.NotFound();
            }

            produtoDB.Nome = produto.Nome;
            produtoDB.Descricao = produto.Descricao;
            produtoDB.Preco = produto.Preco;
            produtoDB.Imagem = produto.Imagem;
            produtoDB.DataCompra = produto.DataCompra;
            produtoDB.Estoque = produto.Estoque;
            produtoDB.CategoriaId = produto.CategoriaId;

            await db.SaveChangesAsync();
            return Results.Ok(produto);
        });

        app.MapDelete("/Produto/{id:int}", async (int id, AppDBContext db) =>
        {
            var produto = await db.Produtos.FindAsync(id);

            if (produto is null)
            {
                return Results.NotFound();
            }

            db.Produtos.Remove(produto);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
