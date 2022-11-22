using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using slowfy_backend.Data;
using slowfy_backend.Services;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<slowfy_backendContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("slowfy_backendContext") ?? throw new InvalidOperationException("Connection string 'slowfy_backendContext' not found.")));

builder.Services.AddDbContext<slowfy_backendContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("slowfy_backendContext") ?? throw new InvalidOperationException("Connection string 'slowfy_backendContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Config:Secret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// adding services
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<ITracksService, TracksService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();