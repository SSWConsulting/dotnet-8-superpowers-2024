# Justice League - Minimal API Demo

Minimal APIs in ASP.NET Core are a streamlined way of building HTTP APIs with minimal overhead and ceremony. They provide a simpler and more concise way to define and configure HTTP endpoints, focusing on reducing boilerplate code and enhancing developer productivity, especially for small-to-medium-sized APIs.

Key Features of Minimal APIs
Simplicity: Minimal APIs are designed to be straightforward and easy to understand, using fewer lines of code to accomplish tasks.
Reduced Boilerplate: Minimal APIs eliminate much of the boilerplate code associated with traditional ASP.NET Core projects, such as controllers, attributes, and extensive configuration.
Endpoint Mapping: Define routes and endpoints directly in the Program.cs file, streamlining the routing setup.
Lightweight: They are ideal for microservices, serverless functions, and small applications where the overhead of the full ASP.NET Core MVC framework is unnecessary.
Full Power of ASP.NET Core: Despite their simplicity, minimal APIs still provide access to the full power of ASP.NET Core, including dependency injection, middleware, and configuration.

## Project Setup

1. Setup the project

    ```bash
    mkdir JusticeLeague
    cd JusticeLeague
    ```

1. Add a dotnet tool manifest:

    ```bash
    dotnet new tool-manifest
    ```

   Reason why we're using a tool manifest is so that we can run `dotnet tool restore` to restore all the tools needed
   for our application.

1. Add the swagger CLI generator:

    ```bash
    dotnet tool install SwashBuckle.AspNetCore.Cli
    ```

1. Create a directory

    ```bash
    mkdir JusticeLeague.Api
    cd JusticeLeague.Api
    ```

1. Create an empty project

    ```bash
    dotnet new web
    ```

1. Add the project to the solution

    ```bash
    cd ..
    dotnet new sln
    dotnet sln add JusticeLeague.Api
    ```

1. Open the solution in Rider

    ```bash
     .\JusticeLeague.sln
    ```

1. Create the wwwroot directory

   Add the following to the following to the csproj file:

    ```xml
    <Target Name="CreateSwaggerJson" AfterTargets="Build" Condition="$(Configuration)=='Debug'">
        <Exec Command="dotnet swagger tofile --output ./wwwroot/swagger.json $(OutputPath)$(AssemblyName).dll v1" WorkingDirectory="$(ProjectDir)" />
    </Target>
    ```

1. Create a `wwwroot` directory in the root of the project

## Setup Data

1. Create a `Models\Hero` class

    ```csharp
    public record Hero(string HeroName, string CivilianName)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
    ```

1. Create `Services\HeroService` class

    ```csharp
    public class HeroService
    {
        private readonly List<Hero> _heroNames =
        [
            new Hero("Superman", "Clark Kent"),
            new Hero("Batman", "Bruce Wayne"),
            new Hero("Wonder Woman", "Diana Prince"),
            new Hero("The Flash", "Barry Allen"),
            new Hero("Cyborg", "Victor Stone"),
            new Hero("Aquaman", "Arthur Curry")
        ];
    
        public IEnumerable<Hero> GetHeroes() => _heroNames;
    
        public void AddHero(Hero hero) => _heroNames.Add(hero);
    
        public Hero? GetHeroById(Guid id) => _heroNames.FirstOrDefault(x => x.Id == id);
    }
    ```

### Add First API

1. Remove the default API

1. Add the following to Program.cs

    ```csharp
    app.MapGet("api/heroes", (HeroService heroService) => heroService.GetHeroes());
    ```

1. Run API

1. Show exception

   > **NOTE**: The exception happened because we need to tell ASP.NET Core where HeroService needs to come from. We'll
   also need to add it to the DI container.

1. Add Hero Service to DI Container

    ```csharp
      builder.Services.AddSingleton<HeroService>();
    ```

1. Show API working

### Add Other APIs

1. Add the following to Program.cs

    ```csharp
    app.MapGet("/api/heroes", (HeroService heroService) => heroService.GetHeroes());
    
    app.MapPost("api/heroes", (string heroName, string civilianName, HeroService heroService) =>
    {
        var hero = new Hero(heroName, civilianName);
        heroService.AddHero(hero);
        return TypedResults.Created($"/heroes/{hero.Id}", hero);
    });
    
    app.MapGet("api/heroes/{id:Guid}", Results<Ok<Hero>, NotFound>(Guid id, HeroService heroService) =>
    {
        var hero = heroService.GetHeroById(id);
        if (hero == null)
            return TypedResults.NotFound();
    
        return TypedResults.Ok(hero);
    });  
    ```

1. Test and show API working

   > **NOTE**: Now we have an API working, but we don't have any documentation for it. Let's add Open API to our
   project.

## Add Open API & Swagger UI

1. Add Swashbuckle Package

    ```bash
    dotnet add package Swashbuckle.AspNetCore
    ```

1. Configure Swashbuckle

    ```csharp
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    ```

1. Show Swagger UI

## Improve Open API Spec ⬇️

1. Add the following to each API

    ```csharp
    .WithName("GetHeroes")
    .WithName("GetHeroById")
    .WithName("CreateHero")
    ```

1. Show how the OperationId is now set in Open API spec

> **NOTE**: The Operation ID comes in very handy when generating client code

1. Add this tag to each API

    ```csharp
    .WithTags("Heroes")
    ```

1. Show how the following groups the API in Swagger UI

1. Show how all the HTTP Status codes in the swagger are correct. That is because we're using typed results.

## Refactor API

We've got a bit of duplication going on at the moment and our minimal APIs aren't as DRY as they could be.  Let's fix that up.

1. Create `Endpoints\HeroesEndpoints`

    ```csharp
    public static class HeroesEndpoints
    {
        public static void AddHeroesEndpoints(this WebApplication webApplication)
        {
            var group = webApplication.MapGroup("api/heroes")
                .WithTags("Heroes");
    
            group
                .MapGet("", (HeroService heroService) => heroService.GetHeroes())
                .WithName("GetHeroes");
    
            group
                .MapPost("", (string heroName, string civilianName, HeroService heroService) =>
                {
                    var hero = new Hero(heroName, civilianName);
                    heroService.AddHero(hero);
                    return TypedResults.Created($"/heroes/{hero.Id}", hero);
                })
                .WithName("CreateHeroes");
    
            group
                .MapGet("{id:Guid}", Results<Ok<Hero>, NotFound> (Guid id, HeroService heroService) =>
                {
                    var hero = heroService.GetHeroById(id);
                    if (hero == null)
                        return TypedResults.NotFound();
    
                    return TypedResults.Ok(hero);
                })
                .WithName("GetHeroById");
        }
    }
    ```

1. Add the following to Program.cs

    ```csharp
    app.AddHeroesEndpoints();
    ```

1. Show APIs still working

## Add Authentication

> **NOTE**: If this application was going to be used by more than one client we would consider using OIDC. However, for
> our use case we only have one client so we're going to use Identity Endpoints and Bearer Tokens.

1. Add Nuget packages

    ```bash
    dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.InMemory
    dotnet add package Microsoft.EntityFrameworkCore.Design
    ```

1. Add Auth Services

    ```csharp
    builder.Services
        .AddAuthentication()
        //.AddCookie("JusticeLeague.Api");
        // NOTE: You swap out BearerToken auth for Cookie auth by using 'AddCookie()`
        .AddBearerToken(IdentityConstants.BearerScheme);
   
    builder.Services.AddAuthorization();
    ```

1. Create `Infrastructure\Identity\ApplicationDbContext`

    ```csharp
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options);
    
    public class ApplicationUser : IdentityUser
    {
    }
    ```

1. Add Identity Endpoints

    ```csharp
    builder.Services.AddIdentityCore<ApplicationUser>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddApiEndpoints();
    ```

1. Add In-Memory DB

    ```csharp
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("justice-league"));
    ```

1. Configure swagger to use bearer tokens

    ```csharp
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
        opt.AddServer(new OpenApiServer() { Description = "Local", Url = "https://localhost:5009/" });
    });

    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 5009;
    });
   
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5008); // HTTP port
        options.ListenAnyIP(5009, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    });

    ```
1. Update the Url above with the URL from the web project

1. Configure the route for our identity endpoints

    ```csharp
    app.MapGroup("account").WithTags("Account").MapIdentityApi<ApplicationUser>();
    ```

1. Add Authentication to our HeroEndpoints

    ```csharp
    .RequireAuthorization()
    ```
