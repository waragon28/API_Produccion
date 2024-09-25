using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using InspeccionProduccion.API.Dependency;
using InspeccionProduccion.API.Filters;
using InspeccionProduccion.API.Middleware;
using InspeccionProduccion.API.Interfaces;
using Produccion.API.Dependency;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddParadaMaquina(builder.Configuration);
    builder.Services.AddAuth(builder.Configuration); 
    builder.Services.AddPalet(builder.Configuration);
    builder.Services.AddEvaluacion(builder.Configuration); 
    builder.Services.AddMaquinista(builder.Configuration);
    builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy=null);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

}

builder.Services.AddControllers();

var secretKey = builder.Configuration.GetSection("settings").GetValue<string>("secretKey");

if (secretKey == null)
{
    throw new ArgumentNullException("secretKey", "La clave secreta no puede ser nula.");
}

var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseExceptionHandler("/error");
    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthentication(); // Asegúrate de que esta línea esté presente si estás usando autenticación
    app.UseAuthorization(); // Asegúrate de que esta línea esté presente

    app.MapControllers();


    app.Run();
}

