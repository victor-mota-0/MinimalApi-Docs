using Application.Consumers.Products;
using Infra.Api.Configuration;
using Infra.Api.Endpoints.Products;

var builder = WebApplication.CreateBuilder(args);

// Configura serviços
builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddLogging();
builder.Services.AddScoped<ProductConsumer>();
builder.Services.AddCap(options =>
{
    options.UsePostgreSql(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
    var rabbitMq = builder.Configuration.GetSection("RabbitMQ");
    options.UseRabbitMQ(options =>
    {
        options.HostName = rabbitMq.GetValue<string>("HostName") ?? string.Empty;
        options.UserName = rabbitMq.GetValue<string>("UserName") ?? string.Empty;
        options.Password = rabbitMq.GetValue<string>("Password") ?? string.Empty;
        options.Port = int.Parse(rabbitMq.GetValue<string>("Port") ?? "5672");
        options.ExchangeName = rabbitMq.GetValue<string>("ExchangeName") ?? string.Empty;
    });
    options.UseDashboard();
});

var app = builder.Build();

// Configura middlewares
app.UseApiConfiguration();

// Registra endpoints por feature
app.MapProductEndpoints();

app.Run();