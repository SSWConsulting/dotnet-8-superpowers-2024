//
// Required & Nullability ----------------------------------------------------------------------------------------------
//

using CSharp11;

var foo = new Foo { Bar = "Bar " };

//
// Raw string literals -------------------------------------------------------------------------------------------------
//
var xml = """
          <html>
          <body>
          <p>Hello, World!</p>
          </body>
          </html>
          """;

// Any number of double quotes can be used as delimiters
// Notice we don't need special handling for the single quotes
var name = "bar";
var sql =
    $$"""""
    SELECT *
    FROM Foo
    WHERE Bar = '{{name}}'
    """"";

//
// Patterns ------------------------------------------------------------------------------------------------------------
//

// Null Pattern
if (foo is not null)
{
    Console.WriteLine(foo.Bar);
}

// Type Pattern
if (foo is Foo)
{
    Console.WriteLine("Foo");
}

// Property Pattern
if (foo is { Bar: "Bar " })
{
    Console.WriteLine("Bar");
}

// Switch Expression
var result1 = foo switch
{
    { Bar: "Foo " } => "Foo",
    { Bar: "Bar" } => "Bar",
    _ => "Default" // Discard Pattern
};

// Relational Pattern
int age = 30;
var result2 = age switch
{
    < 18 => "Child",
    >= 18 and <= 65 => "Adult",
    > 65 => "Senior",
};