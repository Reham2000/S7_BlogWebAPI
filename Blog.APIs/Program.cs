using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Connection

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseLazyLoadingProxies()
    .UseSqlServer(
        builder.Configuration.GetConnectionString("BlogDB")
        )
);

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Request Life Time
//builder.Services.AddScoped<ICategoryService,CategoryService>();
//builder.Services.AddSingleton<ICategoryService,CategoryService>();
//builder.Services.AddTransient<ICategoryService,CategoryService>();
//builder.Services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
