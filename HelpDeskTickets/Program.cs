using AutoMapper;
using HelpDeskTickets.App.MappingProfiles;
using HelpDeskTickets.App.Services;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.EF.Data;
using HelpDeskTickets.EF.Repositories;
using HelpDeskTickets.Services;
using HelpDeskTickets.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings configuration not found");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
        //b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        
        ));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IDepartmentSrevice, DepartmentSrevice>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddSingleton(jwtSettings);

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var secretKeyBytes = System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
var securityKey = new SymmetricSecurityKey(secretKeyBytes);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = securityKey,
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles =
    {
        "Admin",
        "Manager",
        "Customer"
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/secure-data", ()=>"Secert Token Data").RequireAuthorization();
app.MapControllers();

app.Run();