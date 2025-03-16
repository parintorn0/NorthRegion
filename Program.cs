using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NorthRegion.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NorthRegionDbContext>(
    // options => options.UseSqlite(builder.Configuration.GetConnectionString("MyDbSQLite"))
    options => options.UseMySQL("Server=localhost;Database=Product;Uid=root;Pwd=123;")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NorthRegion}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
