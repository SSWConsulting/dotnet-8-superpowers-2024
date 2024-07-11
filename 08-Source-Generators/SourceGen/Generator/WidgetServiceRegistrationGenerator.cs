using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator;


[Generator]
public class WidgetServiceRegistrationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
            .Where(static m => m != null);

        var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, (spc, source) => Execute(source.Left, source.Right, spc));
    }

    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
    {
        var widgetServiceInterface = compilation.GetTypeByMetadataName("IWidgetService");
        if (widgetServiceInterface == null)
        {
            return;
        }

        var registrations = new StringBuilder();
        registrations.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        registrations.AppendLine("public static class WidgetServiceRegistration");
        registrations.AppendLine("{");
        registrations.AppendLine("    public static void AddWidgetServices(this IServiceCollection services)");
        registrations.AppendLine("    {");

        foreach (var classDeclaration in classes)
        {
            var model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var typeSymbol = model.GetDeclaredSymbol(classDeclaration) as ITypeSymbol;
            if (typeSymbol != null && typeSymbol.AllInterfaces.Contains(widgetServiceInterface))
            {
                registrations.AppendLine($"        services.AddSingleton<{widgetServiceInterface.Name}, {typeSymbol.Name}>();");
            }
        }

        registrations.AppendLine("    }");
        registrations.AppendLine("}");

        context.AddSource("WidgetServiceRegistration.g.cs", registrations.ToString());
    }
}
