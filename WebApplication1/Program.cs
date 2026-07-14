using MasterDetail.Frameworks.ResponseFrameworks.Contracts;
using MasterDetail.Repositories.Contract;
using MasterDetail.Repositories.Repository;
using MasterDetails.ApplicationServices.Services;
using MasterDetails.ApplicationServices.Services.Contracts.ContractCustomer;
using MasterDetails.ApplicationServices.Services.Contracts.ContractOrder;
using MasterDetails.ApplicationServices.Services.Contracts.ContractProduct;
using MasterDetails.Frameworks.ResponseFrameworks;
using MasterDetails.Models;
using MasterDetails.Models.Services.Contracts;
using MasterDetails.Models.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region [- DbContext-]

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    options.UseSqlServer(connectionString);

    // فقط در محیط توسعه
    if (builder.Environment.IsDevelopment())
    {
        // نمایش مقدار واقعی پارامترها
        options.EnableSensitiveDataLogging();

        // نمایش Query ها و زمان اجرا
        options.LogTo(
            Console.WriteLine,
            LogLevel.Information);
    }
});

#endregion

#region[- AddScoped() -]
builder.Services.AddScoped<IResponseFactory, ResponseFactory>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductApplicationService, ProductService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerApplicationService, CustomerService>();
#endregion


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
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
