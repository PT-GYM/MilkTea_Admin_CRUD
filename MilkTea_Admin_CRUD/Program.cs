using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MilkTea_Admin_CRUD.Hubs;
using Repository;
using Repository.UnitOfWorks;
using Services;

var builder = WebApplication.CreateBuilder(args);
// Thêm DbContext
builder.Services.AddDbContext<MilkTeaShopContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Thêm Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authen/Login";
        options.AccessDeniedPath = "/AccessDenied";
    });

builder.Services.AddAuthorization();


// Thêm các dịch vụ
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSession();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccount, Account>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IComboService, ComboService>();
builder.Services.AddScoped<IToppingService, ToppingService>();
builder.Services.AddScoped<ICategory, CategoryService>();

var app = builder.Build();

// Cấu hình Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   
app.UseRouting();
app.UseSession();
app.UseAuthentication();  
app.UseAuthorization();
app.MapHub<ProductHub>("/productHub");

app.MapRazorPages();
app.Run();






