using BussinessObject;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MilkTea_Admin_CRUD.Hubs;
using MilkTea_Admin_CRUD.Service;
using Repository;
using Repository.UnitOfWorks;
using Services.Interface;
using Services.Service;

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
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IToppingService, ToppingService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IHubService, HubService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IComboService, ComboService>();


var app = builder.Build();

// Cấu hình Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Authen/Login");
        return;
    }
    await next();
});

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();   
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHub<ProductHub>("/productHub"); 
});

app.UseAuthentication();  
app.UseAuthorization();


app.MapRazorPages();
app.Run();






