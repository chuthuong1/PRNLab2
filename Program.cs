using Microsoft.Extensions.Options;
using System;
using WebApplication1.ChatHubSignarl; // si
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Bussiness.Mapping;
using WebApplication1.Bussiness.Repository;
using WebApplication1.DataAccess.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddRazorPages();
//si
builder.Services.AddSignalR();

builder.Services.AddTransient<IProductRepository, ProductRepository>()
    .AddDbContext<NorthWindContext>(opt => builder.Configuration.GetConnectionString("Northwind"));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSession();
var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
// si
app.MapHub<ChatHub>("/chatHub");

app.Run();
