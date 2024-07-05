# C# Features Demo

## Setup

Create a new console app called CSharp11

```bash
mkdir CSharp
cd CSharp
mkdir CSharp11
cd CSharp11
dotnet new console
```

Create a new console app called CSharp12

```bash
mkdir CSharp12
cd CSharp12
dotnet new console
```

Create a new solution

```bash
dotnet new sln
dotnet sln add .\CSharp11
dotnet sln add .\CSharp12
```

# C# 11

## Records

Create a person record

```csharp
record Person(string FirstName, string LastName);
```

create a new person

```csharp
var p1 = new Person("Daniel", "Mackay");
```

Records are immutable and the following will not work

```csharp
p1.FirstName = "Fred";
```

However, we can update a copy of the person using 'with'

```csharp
var p2 = p1 with { FirstName = "Fred" };
```

We also get value based equality

```csharp
var doppleGanger = new Person("Daniel", "Mackay");
if (p1 == doppleGanger) 
{
    Console.WriteLine("equal")'
}
 
```

## Required and Init Properties

Create the following class

```csharp
public class Foo
{
    public string Bar { get; set; }    
}
```

Notice how we get a warning around 'Bar' being uninitialized.  THis is because we have nullable reference types turned on by default.  THis means the compiler will try to warn us against any properties that could possibly be null.

we can get around this by using the `required` keyword.

```csharp
public class Foo2
{
    public required string Bar { get; set; }
}
```

Now this removes the warning, but it also means we can set this property anywhere in code (which we often don't want). 

```csharp
    var foo = new Foo { Bar = "Bar " };
    foo.Bar = "Bar 2"; 🤢
```

To get around this we can use the `init` keyword.

```csharp
public class Foo
{
    public required string Bar { get; init; }
}
```

Now we have a nicely constructed object that is guaranteed to have a value for `Bar` and we can't change it after construction.

For now, we'll move the class to another file so it doesn't break our demo.

## Raw String Literals

Raw string literals were created to make it easier to have nicely formatted blocks of code from other formats.

```csharp
var xml = """
          <html>
          <body>
          <p>Hello, World!</p>
          </body>
          </html>
          """;
```
We can also use interpolation with these strings.

We can use any number of quotes to start and end the string, as long as the start and ends match.

```csharp

```csharp
var name = "bar";
var sql =
    $$"""""
    SELECT *
    FROM Foo
    WHERE Bar = '{{name}}'
    """"";
```

## Null Pattern

Next we're going to move onto patterns.  There are many different types of patterns in C#.  Some you've most likely used before, but may not realise they're patterns.

First is the null pattern.

```csharp

if (foo is not null)
{
    Console.WriteLine(foo.Bar);
}
```

## Type pattern

```csharp
if (foo is Foo)
{
    Console.WriteLine("Foo");
}
```

## Property pattern

```csharp
if (foo is { Bar: "Bar " })
{
    Console.WriteLine("Bar");
}
```

## Switch expression / Discard Pattern

```csharp
var result1 = foo switch
{
    { Bar: "Foo " } => "Foo",
    { Bar: "Bar" } => "Bar",
    _ => "Default" // Discard Pattern
};
```

## Relational Pattern

```csharp
int age = 30;
var result2 = age switch
{
    < 18 => "Child",
    >= 18 and <= 65 => "Adult",
    > 65 => "Senior",
};
```

## Positional Pattern

Positional patterns allow you to destructure and match patterns based on the properties or elements of an object or tuple

```csharp
var result3 = foo switch
{
    (string bar, _) => bar,
    _ => "Default"
};
```

These patterns can also be combined.  For example, you may use the positional pattern to destructure an object, then the relational pattern to perform tests on the properties.

# C# 12

## Setup

Create a new class library called CSharp12

```bash
mkdir CSharp12
cd CSharp12
dotnet new classlib
```

## Primary Constructors

The major new C# 12 feature added is Primary Constructors.  Let's take a look at what a class looks like with and without this feature.

Create a hero class as follows:

```csharp
public class Hero
{
    private string _name;
    private int _age;
    private string _superPower;

    public Hero(string name, int age, string superPower)
    {
        _name = name;
        _age = age;
        _superPower = superPower;
    }
}
```

Now, if we refactor this to use Primary Constructors, it looks like this:

```csharp
public class Hero2(string name, int age, string superPower)
{
    private string _name = name;
    private int _age = age;
    private string _superPower = superPower;
}
```

This can be especially useful to remove code we'd usually have to deal with dependency injection.

> **NOTE**: Primary Constructors may look similar to records, but they operate quite differently.  With records the params are turned into properties.  With Primary Constructors, they are parameters.

Show how we can't use 'this' to reference the parameters.

## Collection Expressions

Over the years arrays and lists have been getting easier to initialize in C# by becoming simpler to write.

Collection expressions are the next (and probably the last) evolution of this.

We can initialize a list of simple numbers as follows.

```csharp
List<int> numbers = [1, 2, 3, 4, 5];
```

Now let's create 3 functions that take in different types of lists

```csharp
void Foo(IEnumerable<int> numbers)
{
}

void Foo2(List<int> numbers)
{
}

void Foo3(int[] numbers)
{
}
```

we can use list expressions for all of these functions as follows:

```csharp
Foo([1,2,3]);
Foo([]);
Foo2([1,2,3]);
Foo3([1,2,3]);
```

## Spread Operator

The spread operator is a new operator that allows us to join two lists together.

```csharp
List<int> lowNumbers = [1, 2, 3];
List<int> highNumbers = [4, 5, 6];
List<int> allNumbers = [..lowNumbers, ..highNumbers];
```

## Lambda defaults

The last feature we'll look at is lambda defaults.  This is not a huge feature, but may come int hand from time to time.

```csharp
var lambda = (int start = 0, int end = 10) => Console.WriteLine("Start: {0}, End: {1}", start, end);
lambda();
lambda(2, 5);
```
