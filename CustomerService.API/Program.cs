using Cliente.Infrastructure.BackgroundServices;
using CustomerService.Application.Commands;
using CustomerService.Application.Commands.Handlers;
using CustomerService.Domain.Entities.Repositories;
using CustomerService.Domain.Interfaces;
using CustomerService.Infrastructure.Services;
using CustomerService.Infrastructure.Services.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Customer Service API",
        Version = "v1",
        Description = "API para gerenciamento de clientes e integração com proposta de crédito e emissão de cartões"
    });
});

// Registrar MediatR com todos os assemblies necessários
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); // Projeto API
    cfg.RegisterServicesFromAssembly(typeof(CadastroClienteCommand).Assembly); // Projeto Application
    cfg.RegisterServicesFromAssembly(typeof(CadastroClienteCommandHandler).Assembly); // Mesmo assembly do Handler
    cfg.RegisterServicesFromAssembly(typeof(StatusCreditoPropostaEventHandler).Assembly);
});

// Add Infrastructure services
builder.Services.AddInfrastructure();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

// 👉 Adicionando o HostedService (Worker que consome a fila/exchange do RabbitMQ)
builder.Services.AddHostedService<ErroGeracaoCartaoWorker>();

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
