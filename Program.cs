using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;   // ⚠️ Make sure this namespace matches yours

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();
// 1️⃣ Add Razor Pages
builder.Services.AddRazorPages();

// 2️⃣ Add DbContext
builder.Services.AddDbContext<OnlineShopContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 3️⃣ Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.Run();