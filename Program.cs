using ShopperBackend.Models;
using ShopperBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShopperBackend.Middlewares;
using ShopperBackend.Exceptions;
using Microsoft.AspNetCore.Mvc; // Add this using directive



var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ShopperDatabaseSettings>(builder.Configuration.GetSection("ShopperDatabase"));

builder.Services.AddSingleton<ProductsService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<IProductsService, ProductsService>();

// builder.Services.AddExceptionHandler<CustomExceptionHandler>();


builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
    };
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
    var middleware1 = new RequestLoggingMiddleware(next, logger, "Middleware1");
    await middleware1.InvokeAsync(context);
});

//app.UseMiddleware<CustomErrorHandlingMiddleware>();
app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();