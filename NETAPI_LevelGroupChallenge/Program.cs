using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETAPI_LevelGroupChallenge.Data;
using NETAPI_LevelGroupChallenge.DTOs;
using NETAPI_LevelGroupChallenge.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CategoriaDb>(opt =>
        opt.UseOracle(builder.Configuration.GetConnectionString("FiapOracleConnection")));
builder.Services.AddDbContext<TipoProdutoDb>(opt =>
        opt.UseOracle(builder.Configuration.GetConnectionString("FiapOracleConnection")));
var app = builder.Build();


app.MapGet("/categorias", GetAllCategorias).Produces<IList<CategoriaDb>>();
app.MapGet("/categorias/paginable", GetPaginableCategorias).Produces<IList<CategoriaDb>>();
app.MapGet("/categorias/paginable/{searchTerm}", GetPaginableLikeCategorias).Produces<IList<CategoriaDb>>();
app.MapPost("/categorias", CreateCategoria)
    .Accepts<CategoriaDTO>("application/json")
    .Produces<CategoriaDTO>(StatusCodes.Status201Created)
    .WithName("Create Categoria");
app.MapPut("/categorias/{id}", UpdateCategoria);
app.MapDelete("/categorias/{id}", DeleteCategoria);


app.MapGet("/tipoprodutos", GetAllTipoProdutos).Produces<IList<TipoProdutoDb>>();
app.MapGet("/tipoprodutos/paginable", GetPaginableTipoProduto).Produces<IList<TipoProdutoDb>>();
app.MapGet("/tipoprodutos/paginable/{searchTerm}", GetPaginableLikeTipoProduto).Produces<IList<TipoProdutoDb>>();
app.MapPost("/tipoprodutos", CreateTipoProduto)
    .Accepts<TipoProdutoDTO>("application/json")
    .Produces<TipoProdutoDTO>(StatusCodes.Status201Created)
    .WithName("Create TipoProduto");
app.MapPut("/tipoprodutos/{id}", UpdateTipoProduto);
app.MapDelete("/tipoprodutos/{id}", DeleteTipoProduto);




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task<IResult> GetAllCategorias(CategoriaDb db)
{
    return TypedResults.Ok(await db.Categorias
        .Select(categoria => new CategoriaDTO(
            categoria.Id, categoria.Name))
        .ToListAsync());
}
static async Task<IResult> GetPaginableCategorias(CategoriaDb db, [FromQuery]int PageNo = 1, [FromQuery] int PageSize = 10)
{
    return TypedResults.Ok(await db.Categorias.AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip((PageNo - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync()
        );
}
static async Task<IResult> GetPaginableLikeCategorias(String searchTerm, CategoriaDb db, [FromQuery] int PageNo = 1, [FromQuery] int PageSize = 10)
{
    return TypedResults.Ok(await db.Categorias.AsNoTracking()
            .Where(e => EF.Functions.Like(e.Name, $"%{searchTerm}%"))
            .OrderBy(x => x.Id)
            .Skip((PageNo - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync()
        );
}
static async Task<IResult> CreateCategoria(CategoriaDTO categoriaDTO, HttpContext context)
{
    var db = context.RequestServices.GetRequiredService<CategoriaDb>();

    var categoria = new Categoria()
    {
        Name = categoriaDTO.Name
    };

    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/categorias/{categoria.Id}", categoria);
}
static async Task<IResult> UpdateCategoria(int id, CategoriaDTO categoriaDTO, CategoriaDb db)
{
    var categoria = await db.Categorias.FindAsync(id);

    if (categoria is null) return TypedResults.NotFound();

    categoria.Name = categoriaDTO.Name;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}
static async Task<IResult> DeleteCategoria(int id, CategoriaDb db)
{
    if (await db.Categorias.FindAsync(id) is Categoria categoria)
    {
        db.Categorias.Remove(categoria);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
static async Task<IResult> GetAllTipoProdutos(TipoProdutoDb db)
{
    return TypedResults.Ok(await db.TipoProdutos
        .Select(tipoProduto => new TipoProdutoDTO(
            tipoProduto.Id, tipoProduto.Name))
        .ToListAsync());
}
static async Task<IResult> GetPaginableTipoProduto(TipoProdutoDb db, [FromQuery] int PageNo = 1, [FromQuery] int PageSize = 10)
{
    return TypedResults.Ok(await db.TipoProdutos.AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip((PageNo - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync()
        );
}
static async Task<IResult> GetPaginableLikeTipoProduto(String searchTerm, TipoProdutoDb db, [FromQuery] int PageNo = 1, [FromQuery] int PageSize = 10)
{
    return TypedResults.Ok(await db.TipoProdutos.AsNoTracking()
            .Where(e => EF.Functions.Like(e.Name, $"%{searchTerm}%"))
            .OrderBy(x => x.Id)
            .Skip((PageNo - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync()
        );
}
static async Task<IResult> CreateTipoProduto(TipoProdutoDTO tipoProdutoDTO, HttpContext context)
{
    var db = context.RequestServices.GetRequiredService<TipoProdutoDb>();

    var tipoProduto = new TipoProduto()
    {
        Name = tipoProdutoDTO.Name
    };

    db.TipoProdutos.Add(tipoProduto);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/tipoprodutos/{tipoProduto.Id}", tipoProduto);
}

static async Task<IResult> UpdateTipoProduto(int id, TipoProdutoDTO tipoProdutoDTO, TipoProdutoDb db)
{
    var tipoProduto = await db.TipoProdutos.FindAsync(id);

    if (tipoProduto is null) return TypedResults.NotFound();

    tipoProduto.Name = tipoProdutoDTO.Name;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTipoProduto(int id, TipoProdutoDb db)
{
    if (await db.TipoProdutos.FindAsync(id) is TipoProduto tipoProduto)
    {
        db.TipoProdutos.Remove(tipoProduto);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
