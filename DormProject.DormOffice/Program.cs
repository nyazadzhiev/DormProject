using DormProject.DormOffice.Data;
using DormProject.DormOffice.Data.Repositories;
using DormProject.DormOffice.Messages;
using DormProject.DormOffice.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using static MassTransit.Logging.OperationName;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddHttpContextAccessor()
    .AddScoped<DbContext, DormDbContext>()
    .AddDbContext<DormDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddTransient<IStudentRepository, StudentRepository>()
    .AddTransient<IStudentService, StudentService>()
        .AddMassTransit(mt =>
        {
            mt.AddConsumer(typeof(StudentAddedConsumer));

            mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rmq =>
            {
                rmq.Host("localhost");

                rmq.ReceiveEndpoint(nameof(StudentAddedConsumer), endpoint =>
                {
                    endpoint.ConfigureConsumer(bus, typeof(StudentAddedConsumer));
                });
            }));
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

app.UseAuthorization();

app.MapControllers();

using var serviceScope = app.Services.CreateScope();

var serviceProvider = serviceScope.ServiceProvider;

var db = serviceProvider.GetRequiredService<DormDbContext>();

if (db.Database.EnsureCreated())
{
    RelationalDatabaseCreator databaseCreator =
    (RelationalDatabaseCreator)db.Database.GetService<IDatabaseCreator>();
    //databaseCreator.CreateTables();
    db.Database.Migrate();
}

app.Run();
