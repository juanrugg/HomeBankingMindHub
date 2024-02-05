using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x=>
x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.AddScoped<IClientRepository, ClientRepository>();

// Add services to the container.
builder.Services.AddRazorPages();

//Add DbContext to the container

builder.Services.AddDbContext<HomeBankingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingConexion")));

var app = builder.Build();

//crete a scope to get the DbContext instance

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {

        // En este paso buscamos un service que este con la clase HomeBankingContext
        var context = services.GetRequiredService<HomeBankingContext>();
        DBInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ha ocurrido un error al enviar la información a la base de datos!");
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.Run();
