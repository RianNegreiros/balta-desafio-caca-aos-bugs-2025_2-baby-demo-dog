using System;
using BugStore.Data;
using BugStore.Endpoints;
using BugStore.Services.Customers;
using BugStore.Services.Products;
using BugStore.Services.Orders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapCustomerEndpoints();

app.MapProductEndpoints();

app.MapOrderEndpoints();

app.Run();
