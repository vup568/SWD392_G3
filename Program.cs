using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWD392_MVC.Models;
using SWD392_MVC.Repositories;
using SWD392_MVC.Services;

// Vietnamese / Unicode encoding support
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// ── MVC ──────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ── Session (cart) ────────────────────────────────────────────────────────────
builder.Services.AddSession(options =>
{
    options.IdleTimeout        = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly    = true;
});
builder.Services.AddHttpContextAccessor();

// ── Database ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<OnlineShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew                = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                ctx.Token = ctx.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    });

// ── Repositories ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository,            UserRepository>();
builder.Services.AddScoped<IRoleRepository,            RoleRepository>();
builder.Services.AddScoped<IProductRepository,         ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IOrderRepository,           OrderRepository>();
builder.Services.AddScoped<IFeedbackRepository,        FeedbackRepository>();
builder.Services.AddScoped<IPostRepository,            PostRepository>();
builder.Services.AddScoped<ISliderRepository,          SliderRepository>();
builder.Services.AddScoped<ISettingRepository,         SettingRepository>();

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IJwtService,           JwtService>();
builder.Services.AddScoped<IAccountService,       AccountService>();
builder.Services.AddScoped<IProductService,       ProductService>();
builder.Services.AddScoped<IOrderService,         OrderService>();
builder.Services.AddScoped<IBlogService,          BlogService>();
builder.Services.AddScoped<IMarketingService,     MarketingService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IDashboardService,     DashboardService>();

// ── Pipeline ──────────────────────────────────────────────────────────────────
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name:    "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name:    "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
