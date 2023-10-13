using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Novus_Top_Trumps.Data;
using Novus_Top_Trumps.Models;
using System;
using System.Linq;
using Novus_Top_Trumps.Models.Card_Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CardsDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CardsDBContext") ?? throw new InvalidOperationException("Connection string 'CardsDBContext' not found.")));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

PopulateCardsDb(app);

app.Run();


void PopulateCardsDb(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<CardsDBContext>();

    if (!dbContext.CarsCard.Any())
    {
       foreach (var card in CarCardData.CarDeck)
        {
            dbContext.CarsCard.Add(card);
        }
       dbContext.SaveChanges();
    }
    if (!dbContext.PokemonCard.Any())
    {
        foreach (var card in PokemonCardData.PokemonDeck)
        {
            dbContext.PokemonCard.Add(card);
        }
        dbContext.SaveChanges();
    }
}
