using DormProject;
using DormProject.Identity.Data;
using DormProject.Identity.Services;
using DormProject.Middleware;
using DormProject.Services.Identity;
using DormProject.UniversityOffice.Data;
using DormProject.UniversityOffice.Data.Repositories;
using DormProject.UniversityOffice.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

JwtBearerEvents? events = null;

var secret = builder.Configuration
    .GetSection(nameof(ApplicationSettings))
    .GetValue<string>(nameof(ApplicationSettings.Secret));

var key = Encoding.ASCII.GetBytes(secret);

builder.Services
    .AddHttpContextAccessor()
    .AddScoped<DbContext, StudentDbContext>()
    .AddDbContext<StudentDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddScoped<ICurrentUserService, CurrentUserService>()
    .AddTransient<IStudentRepository, StudentRepository>()
    .AddTransient<IStudentService, StudentService>()
    .AddMassTransit(mt =>
    {

        mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rmq =>
        {
            rmq.Host("localhost");
        }));
    })
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
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using var serviceScope = app.Services.CreateScope();

var serviceProvider = serviceScope.ServiceProvider;

var db = serviceProvider.GetRequiredService<StudentDbContext>();

if (db.Database.EnsureCreated())
{
    RelationalDatabaseCreator databaseCreator =
    (RelationalDatabaseCreator)db.Database.GetService<IDatabaseCreator>();
    //databaseCreator.CreateTables();
    db.Database.Migrate();
}

app.Run();
