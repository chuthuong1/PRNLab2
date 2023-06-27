using Microsoft.Extensions.Options;
using System;
using WebApplication1.ChatHubSignarl; // si
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Bussiness.Mapping;
using WebApplication1.Bussiness.Repository;
using WebApplication1.DataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddTransient<IProductRepository, ProductRepository>()
    .AddDbContext<NorthWindContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Northwind")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSession();

var app = builder.Build();
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");
app.MapHub<CartHub>("/cartHub");

app.Run();