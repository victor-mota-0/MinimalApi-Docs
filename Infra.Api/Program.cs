using Infra.Api.Configuration;
using Infra.Api.Endpoints.Products;

var builder = WebApplication.CreateBuilder(args);

// Configura servi�os
builder.Services.AddApiConfiguration(builder.Configuration);

var app = builder.Build();

// Configura middlewares
app.UseApiConfiguration();

// Registra endpoints por feature
app.MapProductEndpoints();

app.Run();