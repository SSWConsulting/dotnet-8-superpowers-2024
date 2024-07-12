using JusticeLeague.Api.Endpoints;
using JusticeLeague.Api.Infrastructure.Identity;
using JusticeLeague.Api.Models;
using JusticeLeague.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<HeroService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication()
    //.AddCookie("JusticeLeague.Api");
    // NOTE: You swap out BearerToken auth for Cookie auth by using 'AddCookie()`
    .AddBearerToken(IdentityConstants.BearerScheme);
   
builder.Services.AddAuthorization();

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("justice-league"));

var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token based security"
};
    
var securityReq = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        },
        new string[] { }
    }
};
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", securityScheme);
    opt.AddSecurityRequirement(securityReq);
    opt.AddServer(new OpenApiServer() { Description = "Local", Url = "https://localhost:7277/" });
});

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7277;
});
   
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5079); // HTTP port
    options.ListenAnyIP(7277, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

var app = builder.Build();

app.AddHeroesEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("account").WithTags("Account").MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();

app.Run();
