# HeroFinder Demo

In this demo we are going to build an enterprise console application that allows us to find heroes who are available to help us.

## Setup

Create the project structure

```bash
mkdir HeroFinder
cd HeroFinder
dotnet new console -o HeroFinder.Console
cd ..
dotnet new sln
dotnet sln add HeroFinder.Console/HeroFinder.Console.csproj
```

```bash
dotnet add package Spectre.Console.Cli
```

```bash
dotnet add package Microsoft.Extensions.Hosting
```

## Create the app

The first important thing we need to do in an enterprise app, is add some nice ASCII art, so let's do that

```csharp
AnsiConsole.Write(new FigletText("Hero Finder").Color(Color.Purple));
AnsiConsole.WriteLine();
```

Run and test.

TODO: Finish demo...
