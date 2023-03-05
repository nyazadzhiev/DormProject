using DormProject.Identity.Data;
using DormProject.Identity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using DormProject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DormProject.Services.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using DormProject.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var secret = builder.Configuration
    .GetSection(nameof(ApplicationSettings))
    .GetValue<string>(nameof(ApplicationSettings.Secret));

var key = Encoding.ASCII.GetBytes(secret);

JwtBearerEvents events = null;

builder.Services
    .AddHttpContextAccessor()
    .AddScoped<DbContext, IdentityDbContext>()
    .AddScoped<ICurrentUserService, CurrentUserService>()
    .AddScoped<IIdentityService, IdentityService>()
    .AddTransient<ITokenGeneratorService, TokenGeneratorService>()
    .AddDbContext<IdentityDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .Configure<ApplicationSettings>(
                    builder.Configuration.GetSection(nameof(ApplicationSettings)),
                    config => config.BindNonPublicProperties = true)
    .AddAuthentication(authentication =>
    {
        authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    if (events != null)
                    {
                        bearer.Events = events;
                    }
                });

builder.Services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseCors(options => options
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod())
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints => endpoints
        .MapControllers());

using var serviceScope = app.Services.CreateScope();

var serviceProvider = serviceScope.ServiceProvider;

var db = serviceProvider.GetRequiredService<IdentityDbContext>();

if (db.Database.EnsureCreated())
{
    RelationalDatabaseCreator databaseCreator =
    (RelationalDatabaseCreator)db.Database.GetService<IDatabaseCreator>();
    //databaseCreator.CreateTables();

    DbSeeder.SeedUsers(db);
}

app.Run();
