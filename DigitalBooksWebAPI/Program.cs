using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot().AddPolly();

builder.Services.AddDbContext<DigitalBooksContext>(options => options.
UseSqlServer(builder.Configuration.GetConnectionString("conn")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["jwt:Issuer"],
            ValidAudience = builder.Configuration["jwt:Aud"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]))
        };
    });


builder.Services.AddSingleton<ITokenService>(new TokenService());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseOcelot().Wait();

app.MapPost("/validate", [AllowAnonymous] (UserValidationRequestModel request, HttpContext http, ITokenService tokenService) =>
{
    var userName = request.UserName;
    var password = PasswordEncryptionAndDecryption.EncodePasswordToBase64(request.Password);
    var user = new UserValidationCheck(userName, password);
    var isValidUser = user.IsValidUser();
    if (isValidUser)
    {
        var token = tokenService.buildToken(builder.Configuration["jwt:key"],
                                            builder.Configuration["jwt:issuer"],
                                             new[]
                                            {
                                                 builder.Configuration["jwt:Aud"]
                                             },
                                             request.UserName);

        return new
        {
            Token = token,
            IsAuthenticated = true
        };
    }
    return new
    {
        Token = string.Empty,
        IsAuthenticated = false
    };
}).WithName("validate");

app.UseHttpsRedirection();

app.UseAuthorization();



app.MapControllers();

app.Run();
