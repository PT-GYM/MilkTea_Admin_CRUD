using BussinessObject;
using Microsoft.EntityFrameworkCore;
using MilkTea_Customer_.Hubs;
using MilkTea_Customer_.Service;
using Repository.UnitOfWorks;
using Services.Interface;
using Services.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MilkTeaShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IToppingService, ToppingService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IComboService, ComboService>();
var app = builder.Build();
var serviceProvider = builder.Services.BuildServiceProvider();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authen}/{action=Login}/{id?}");

app.Run();
