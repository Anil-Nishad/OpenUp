using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenUpData;
using OpenUpData.Helpers;
using OpenUpData.Models;
using OpenUpData.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Database Configuration
var dbConnectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<OpenUpContext>(options =>
    options.UseSqlServer(dbConnectionString));

//Services configuration
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IHashtagsService, HashtagsService>();
builder.Services.AddScoped<IStoriesService, StoriesService>();
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<IUsersService, UsersService>();

//Identity configuration
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<OpenUpContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Initialize the database with sample data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OpenUpContext>();
    await dbContext.Database.MigrateAsync(); // Ensure the database is created and migrations are applied
    await DbInitializer.Seed(dbContext);
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
