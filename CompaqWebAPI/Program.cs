using CompaqWebAPI.Core;
using CompaqWebAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<InitSDKActionFilter>();
//builder.Services.AddComercialSDKServicesServiceCollection();
builder.Services.AddFacturaElectronicaSDKServicesServiceCollection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSingleRequestMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
