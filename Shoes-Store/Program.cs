using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(
    dbContextOptions => dbContextOptions.UseSqlServer(
        builder.Configuration.GetConnectionString("SQLServerConnection")
    )
    // Opsi debugging ini bisa tetap digunakan
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);
// Add services to the container.
builder.Services.AddControllersWithViews();



// register service
builder.Services.AddScoped<IAccountAdmin, AccountAdminService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<IOrder, OrderService>();
builder.Services.AddScoped<IOrderDetail, OrderDetailService>();
builder.Services.AddScoped<IPayment, PaymentService>();
builder.Services.AddScoped<IProduct, ProductService>();
builder.Services.AddScoped<IProductSize, ProductSizeService>();
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IUserSaldo, UserSaldoService>();

builder.Services.AddSession();


//Konfigurasi Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Konfigurasi session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Konfigurasi cookie authentication
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Authentication/Login";
//        options.AccessDeniedPath = "/Home/AccessDenied";
//        options.SlidingExpiration = true; // Reset waktu kadaluarsa setiap kali pengguna aktif
//        options.Cookie.HttpOnly = true; // Cookie tidak bisa diakses melalui JavaScript
//        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Cookie hanya dikirim melalui HTTPS
//    });

// Kebijakan Authorization
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
//    options.AddPolicy("RequireAnyRole", policy => policy.RequireRole("Admin", "User"));
//});





var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
