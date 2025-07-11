using DotNetEnv;
using MassTransit;
using Pagos.Application.CommandHandler;
using Pagos.Application.Interfaces;
using Pagos.Domain.Events;
using Pagos.Domain.Events.EventHandler;
using Pagos.Domain.Interfaces;
using Pagos.Infrastructure.Configurations;
using Pagos.Infrastructure.Consumers;
using Pagos.Infrastructure.Interfaces;
using Pagos.Infrastructure.Persistences.Repositories.MongoRead;
using Pagos.Infrastructure.Persistences.Repositories.MongoWrite;
using Pagos.Infrastructure.Queries.QueryHandler;
using Pagos.Infrastructure.Services;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar configuración de MongoDB
builder.Services.AddSingleton<MongoWritePagoDbConfig>();
builder.Services.AddSingleton<MongoReadPagoDbConfig>();
builder.Services.AddSingleton<IRestClient>(new RestClient());

// REGISTRA EL REPOSITORIO ANTES DE MediatR
builder.Services.AddScoped<IPagoRepository, PagoWriteRepository>();
builder.Services.AddScoped<IPagoReadRepository, PagoReadRepository>();
builder.Services.AddScoped<IPaymentService, StripeService>();

// REGISTRA MediatR PARA TODOS LOS HANDLERS
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPagoPorIdQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPagoPorIdSubastaQueryHandler).Assembly));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AgregarPagoCommandHandler).Assembly));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AgregarPagoEventHandler).Assembly));

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<AgregarPagoConsumer>();

    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(Environment.GetEnvironmentVariable("RABBIT_URL")), h =>
        {
            h.Username(Environment.GetEnvironmentVariable("RABBIT_USERNAME"));
            h.Password(Environment.GetEnvironmentVariable("RABBIT_PASSWORD"));
        });

        configurator.ReceiveEndpoint(Environment.GetEnvironmentVariable("RABBIT_QUEUE_AgregarPago"), e => {
            e.ConfigureConsumer<AgregarPagoConsumer>(context);
        });

        configurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        configurator.ConfigureEndpoints(context);
    });
});
EndpointConvention.Map<AgregarPagoEvent>(new Uri("queue:" + Environment.GetEnvironmentVariable("RABBIT_QUEUE_AgregarPago")));


// Configuración CORS permisiva
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite cualquier dominio
            .AllowAnyMethod()  // GET, POST, PUT, DELETE, etc.
            .AllowAnyHeader(); // Cualquier cabecera
    });
});

var app = builder.Build();

// Habilitar CORS
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
