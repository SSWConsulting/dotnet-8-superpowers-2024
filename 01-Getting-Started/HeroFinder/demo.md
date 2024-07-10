# HeroFinder Demo

a lot of us have experience building web apps, When we create web apps, we get so many things by default.  DI, logging, configuration. But when building console apps using templates, you get basically nothing.

Today we will look at how we take all the concepts that we get from web apps and apply them to a console app, so we end up with an
enterprise style console application that allows us to find heroes who are available to help us.


Run app - ascii art

top level - all commands
next level heroes - only 2 sub commands
command branching to group together functionality 

> Heroes list
nicely formatted table -> lib is Spectre console
parse input and outputs. Dynamic outputs


builder - hooks ups all our cross cutting concerns DI, logging etc
web application 
host.createapplicationbuilder -> gives us all the tools, web server


Setup code - plumbing, standard code that was copy and pasted
DI injection 

var registrar = new TypeRegistrar(builder); - services it needs to put in our container
var app = new CommandApp(registrar); - pulling types out of our container
























TODO: Do we want to build it from scratch?
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
