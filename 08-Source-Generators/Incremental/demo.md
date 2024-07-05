# Incremental Source Generator Demo


## Setup

Create a console app

bash:
```bash
mkdir Incremental
cd Incremental
dotnet new console --name Incremental.Console
dotnet new sln
dotnet sln add Incremental.Console
```
Create two new empty classes called 'Foo' and 'Bar'

Create reflection code that will output the names of these classes

```csharp
using System.Reflection;

// Load the assembly
Assembly assembly = Assembly.Load("Incremental.Console");

// Get all types in the assembly
Type[] types = assembly.GetTypes();

// Output the class names to the console
foreach (Type type in types)
{
    Console.WriteLine(type.FullName);
}
```

Publish the output

```bash
dotnet publish -o ./publish/normal
```

Execute the published output and show the class names are displayed

Enable AoT compilation by updating the csproj file

```xml
<PublishAot>true</PublishAot>
```

Publish again

```bash
dotnet publish -o ./publish/normal
```

Run the program and show the class names are not displayed

## Add Source Generation

Add a new project to the solution and set the target framework

```bash
dotnet new classlib --name Incremental.SourceGenerator
dotnet sln add Incremental.SourceGenerator
```

Update the the source generator to use .NET Standard

```xml
<TargetFramework>netstandard2.0</TargetFramework>
```

Add the source generator packages

```bash
dotnet add package Microsoft.CodeAnalysis.Analyzers
dotnet add package Microsoft.CodeAnalysis.CSharp
```

Add the source generator code

```csharp
[Generator]
public class MyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        throw new NotImplementedException();
    }
}
```

Add a flag to the csproj to get rid of the warning

```xml
<EnforceExtendedAnalyzerRules>true</EnforceExtendedAnalyzerRules>
```

Create the base of our code

```csharp
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
                // This is what I care about
                predicate: static (node, _) => node is ClassDeclarationSyntax,

                // This is how get the thing I'm looking for
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node
            )
            .Where(m => m is not null);

        var compilation = context.CompilationProvider.Combine(provider.Collect());

        context.RegisterSourceOutput(compilation, Execute);
    }
```

Add the Execute method.  First let's just get some hardcoded output

```csharp
    private void Execute(SourceProductionContext context, (Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
    {
        var theCode =
            $$"""
            namespace ClassListGenerator;
            
            public static class ClassNames
            {
                public static List<string> Names = new()
                {
                    ""Foo"",
                    ""Bar""
                };
            }
            """;

        context.AddSource("YourClassList.g.cs", theCode);
    }
```
Lets update our main program to use the generated code

```csharp
void PrintClassNamesViaSourceGenerator()
{
    if (ClassListGenerator.ClassNames.Names is not null)
    {
        foreach (var name in ClassListGenerator.ClassNames.Names)
        {
            Console.WriteLine(name);
        }
    }
}
```
Try to build the project and show the error

We need to adjust the project reference in order for the source generator to work

```xml
<ProjectReference Include="..\Incremental.SourceGenerator\Incremental.SourceGenerator.csproj" OutputItemType="Analyzer" />
```
> **NOTE:** Sometimes source generators seems to be buggy within Rider.  May need to switch to VS Preview for this.

Build the project and show the output

Now let's make our code dynamic by using the classes we found

```csharp
 private void Execute(SourceProductionContext context,
        (Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
    {
        // new code start 👇
        var (compilation, list) = tuple;

        var nameList = new List<string>();

        foreach (var syntax in list)
        {
            var symbol = compilation
                .GetSemanticModel(syntax.SyntaxTree)
                .GetDeclaredSymbol(syntax) as INamedTypeSymbol;

            nameList.Add($"\"{symbol.ToDisplayString()}\"");
        }

        var names = String.Join(",\n    ", nameList);

        var theCode =
            $$"""
            namespace ClassListGenerator;
            
            public static class ClassNames
            {
                public static List<string> Names = new()
                {
                    {{names}}
                };
            }
            """;
        // new code end 👆
        
        context.AddSource("YourClassList.g.cs", theCode);
    }
```

publish the project again

```bash
dotnet publish -o ./publish/aot
```

run the published output and ensure that we now have the class names correctly generated
