# Justice League - Client Code Gen Demo

## Setup

1. In the solution root, Install the Kiota CLI

    ```bash
    dotnet tool install Microsoft.OpenApi.Kiota
    ```

1. Create the directory structure

    ```bash
    mkdir JusticeLeague.Client
    cd JusticeLeague.Client
    ```

1. Create a console app

    ```bash
    dotnet new console 
    ```
   
1. Add the console to our solution

    ```bash
    cd ..
    dotnet sln add .\JusticeLeague.Client
    ```
   
1. Add the Kiota nuget packages

    ```bash
    dotnet add package Microsoft.Kiota.Abstractions
    dotnet add package Microsoft.Kiota.Http.HttpClientLibrary
    dotnet add package Microsoft.Kiota.Serialization.Form
    dotnet add package Microsoft.Kiota.Serialization.Json
    dotnet add package Microsoft.Kiota.Serialization.Text
    dotnet add package Microsoft.Kiota.Serialization.Multipart
    ```
   
## Generate Client Code

1. Generate the full client code using Kiota

    ```bash
    dotnet kiota generate -l CSharp -c HeroesClient -n JusticeLeague.Client --openapi "..\JusticeLeague.Api\wwwroot\swagger.json" -o .\Client 
    ```

    See how much code is generated


1. Generate the client code using Kiota CLI

    ```bash
    dotnet kiota generate --include-path **/heroes --include-path **/login --include-path **/register -l CSharp -c HeroesClient -n JusticeLeague.Client --openapi "..\JusticeLeague.Api\wwwroot\swagger.json" -o ./Client --clean-output
    ```
   
    Now look at the code generated and see the size difference

1. Add the .net microsoft host nuget package

    ```bash
    dotnet add package Microsoft.Extensions.Hosting
    ```

1. Bootstrap the application in `Program.cs`

    ```csharp
    var builder = Host.CreateApplicationBuilder();
    builder.Configuration.AddUserSecrets<Program>();
    ```

1. Add `ApiConfig`

    ```csharp
    public record ApiConfig
    {
        public const string SectionName = "Api";
    
        [Required]
        public required string Url { get; init; }
    
        [Required]
        public required string UserName { get; init; }
    
        [Required]
        public required string Password { get; init; }
    }
    ```
 
## Authentication
  
   We'll use this to store the configuration for out API

1. Try to manually create and use the `HeroClient`.  Show how many classes we need to instantiate in order to set this up.

1. Create an `AccessTokenProvider`

    ```csharp
    public class AccessTokenProvider(
        IOptions<ApiConfig> apiConfig,
        [FromKeyedServices(HeroClientTypes.Anonymous)]
        HeroesClient client)
        : IAccessTokenProvider
    {
        public async Task<string> GetAuthorizationTokenAsync(
            Uri requestUri,
            Dictionary<string, object>? options = default,
            CancellationToken cancellationToken = default)
        {
            var token = await client.Account.Login.PostAsync(
                new() { Email = apiConfig.Value.UserName, Password = apiConfig.Value.Password },
                cancellationToken: cancellationToken);
            return token?.AccessToken ?? string.Empty;
        }
    
        public AllowedHostsValidator AllowedHostsValidator { get; } = new();
    }
    ```
1. Create an enum for different client types

    ```csharp
    public enum HeroClientTypes
    {
        Anonymous,
        Secure
    }
    ```

1. Register our `AccessTokenProvider`

    ```csharp
    builder.Services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
    ```

1. Register an anonymous client

    ```csharp
    builder.Services.AddKeyedSingleton<HeroesClient>(HeroClientTypes.Anonymous, (p, o) =>
    {
        var authProvider = new AnonymousAuthenticationProvider();
        var adapter = new HttpClientRequestAdapter(authProvider);
        return new HeroesClient(adapter);
    });
    ```
   
1. Register a secure client

    ```csharp
    builder.Services.AddKeyedSingleton<HeroesClient>(HeroClientTypes.Secure, (sp, o) =>
    {
        var accessTokenProvider = sp.GetRequiredService<IAccessTokenProvider>();
        var authProvider = new BaseBearerTokenAuthenticationProvider(accessTokenProvider);
        var adapter = new HttpClientRequestAdapter(authProvider);
        return new HeroesClient(adapter);
    });
    ```

1. Bind our configuration

    ```csharp
    builder.Services
        .AddOptionsWithValidateOnStart<ApiConfig>()
        .BindConfiguration(ApiConfig.SectionName)
        .ValidateDataAnnotations()
        ;
    ```
   
1. Install the options validation package

    ```csharp
    dotnet add package Microsoft.Extensions.Options.DataAnnotations
    ```
   
1. Build and start our app

    ```csharp
    var app = builder.Build();
    
    app.Start();
   
    Console.ReadKey();
    ```

1. Run and test our application

    BOOM! ?? What's happened here?  Our configuration validation had detected we forgot to add our configuration to our user secrets.  Let's fix that.

1. Add the following to user secrets

    ```json
    {
      "Api": {
        "Url": "http://localhost:7140/",
        "UserName": "dan@ssw.com",
        "Password": "Password1?"
      }
    }
    ```

    NOTE: The URL needs to be replaced with the URL of your API

1. Run and test our application again.  Ensure everything starts correctly

## Calling the API

1. Register a user

    ```csharp
    var apiConfig = app.Services.GetRequiredService<IOptions<ApiConfig>>();
    var anonymousClient = app.Services.GetRequiredKeyedService<HeroesClient>(HeroClientTypes.Anonymous);
    await anonymousClient.Account.Register.PostAsync(new RegisterRequest
    {
        Email = apiConfig.Value.UserName, Password = apiConfig.Value.Password
    });
    ```
   
2. Call our secure API

    ```csharp
    var secureClient = app.Services.GetRequiredKeyedService<HeroesClient>(HeroClientTypes.Secure);
    var heroes = await secureClient.Heroes.GetAsync();
    foreach (var hero in heroes)
    {
        Console.WriteLine($"Hero - {hero.HeroName} ({hero.CivillianName})");
    }
    
    Console.ReadKey();
    ```

1. Start both applications at the same time and debug to ensure everything works

# TODO

- Finish the demo
