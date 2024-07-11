using System.Reflection;

//PrintClassNamesViaReflection();
PrintClassNamesViaSourceGenerator();

void PrintClassNamesViaReflection()
{
    // Load the assembly
    Assembly assembly = Assembly.Load("Incremental.Console");

    // Get all types in the assembly
    Type[] types = assembly.GetTypes();

    // Output the class names to the console
    foreach (Type type in types)
    {
        Console.WriteLine(type.FullName);
    }
}

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