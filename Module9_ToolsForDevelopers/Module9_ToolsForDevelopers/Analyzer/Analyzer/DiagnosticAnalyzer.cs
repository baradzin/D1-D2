using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Reflection;

namespace Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Analyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule1 = new DiagnosticDescriptor(DiagnosticId,
            "Inherited member has incorrect name", 
            "Member {0} must ends with Controller", Category, DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Inherited members from System.Web.Controller must ends with Controller");

        private static DiagnosticDescriptor Rule2 = new DiagnosticDescriptor(DiagnosticId,
            "Incorrect Attributes", 
            "Member {0} must be declared with [Authorize] atr, or all public methods",
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "All controllers must be declared with attr Authorize");

        private static DiagnosticDescriptor Rule3 = new DiagnosticDescriptor(DiagnosticId,
            "Incorrect object in Entities namespace",
            "Member {0} must be declared as public, contains prop Id and Name with [DataContract]",
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "All entities in xxx.Entities namespace must be declared as public, contains prop Id and Name with [DataContract]");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(Rule1, Rule2, Rule3);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(FindIncorrectNamedController, SymbolKind.NamedType);
            context.RegisterSymbolAction(AllControllersMustHaveAttributeAuthorize, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalazyClassesInEntityNamespaces, SymbolKind.NamedType);
        }

        /// <summary>
        /// Х	¬се классы, наследуемые от System.Web.Controller должны иметь суффикс Controller
        /// </summary>
        /// <param name="context"></param>
        private static void FindIncorrectNamedController(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var mvcController = context.Compilation.GetTypeByMetadataName("System.Web.Mvc.Controller");
            //var ourBaseController = context.Compilation.GetTypeByMetadataName("WebApplication.Controllers.BaseController");

            var baseTypes = GetBaseClasses(namedTypeSymbol, context.Compilation.ObjectType);
            if (baseTypes.Contains(mvcController))
            {
                if (!namedTypeSymbol.Name.EndsWith("Controller"))
                {
                    var diagnostic = Diagnostic.Create(Rule1, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        /// <summary>
        /// Х	¬се контроллеры должны быть размечены атрибутом 
        /// [Authorize] Ц либо весь класс целиком, либо все публичные методы
        /// </summary>
        /// <param name="context"></param>
        private static void AllControllersMustHaveAttributeAuthorize(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var mvcController = context.Compilation.GetTypeByMetadataName("System.Web.Mvc.Controller");
            string attr = "AuthorizeAttribute";
            bool IsWarningSymbol = false;

            var baseTypes = GetBaseClasses(namedTypeSymbol, context.Compilation.ObjectType);
            if (baseTypes.Contains(mvcController))
            {
                var publicMethods = namedTypeSymbol.GetMembers()
                        .Where(x => x.Kind == SymbolKind.Method
                        && x.DeclaredAccessibility == Accessibility.Public
                        && !x.IsOverride);
                if (HasAttribute(namedTypeSymbol, attr)) {
                    if (publicMethods.Any(x => HasAttribute(x, attr))) IsWarningSymbol = true;
                } else {
                    if (!publicMethods.All(x => HasAttribute(x, attr))) IsWarningSymbol = true;
                }

                if (IsWarningSymbol)
                {
                    var diagnostic = Diagnostic.Create(Rule2, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static void AnalazyClassesInEntityNamespaces(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            bool IsWarning = false;

            if(namedTypeSymbol.ContainingNamespace.Name.EndsWith("Entities"))
            {
                if(namedTypeSymbol.DeclaredAccessibility != Accessibility.Public)
                {
                    IsWarning = true;
                } else
                {
                    var idProp = namedTypeSymbol.GetMembers("Id").FirstOrDefault();
                    var nameProp = namedTypeSymbol.GetMembers("Name").FirstOrDefault();
                    if(idProp != null && nameProp != null)
                    {;
                        if ((idProp.DeclaredAccessibility & nameProp.DeclaredAccessibility )!= Accessibility.Public 
                            || !idProp.GetAttributes().Any(x => x.AttributeClass.Name.Equals("DataContractAttribute")) 
                            || !nameProp.GetAttributes().Any(x => x.AttributeClass.Name.Equals("DataContractAttribute")))
                        {
                            IsWarning = true;
                        }
                    } else
                    {
                        IsWarning = true;
                    }
                }

                if (IsWarning)
                {
                    var diagnostic = Diagnostic.Create(Rule3, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static bool HasAttribute(ISymbol symbol, string attrName)
        {
            return symbol.GetAttributes().Any(x => x.AttributeClass.Name.Equals(attrName));
        }

        public static ImmutableArray<INamedTypeSymbol> GetBaseClasses(INamedTypeSymbol type, INamedTypeSymbol objectType)
        {
            if (type == null || type.TypeKind == TypeKind.Error)
                return ImmutableArray<INamedTypeSymbol>.Empty;

            if (type.BaseType != null && type.BaseType.TypeKind != TypeKind.Error)
                return GetBaseClasses(type.BaseType, objectType).Add(type.BaseType);

            return ImmutableArray<INamedTypeSymbol>.Empty;
        }
    }
}
