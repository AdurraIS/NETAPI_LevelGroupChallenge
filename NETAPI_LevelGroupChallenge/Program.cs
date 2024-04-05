using Microsoft.EntityFrameworkCore;
using NETAPI_LevelGroupChallenge;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CategoriaDb>(opt =>
        opt.UseOracle(builder.Configuration.GetConnectionString("FiapOracleConnection")));
var app = builder.Build();


app.MapGet("/categorias", GetAllCategorias).Produces<IList<CategoriaDb>>();
app.MapPost("/categorias", CreateCategoria)
    .Accepts<CategoriaDTO>("application/json")
    .Produces<CategoriaDTO>(StatusCodes.Status201Created)
    .WithName("Create Categoria");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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