using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public class FunctionBuilder : BuilderBase<CppFunction>
    {
        public FunctionBuilder(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap) : base(workspace, projectId, directory, typeMap)
        {
        }

        public override void Build(IEnumerable<CppFunction> cppFunctions)
        {
            var firstFunction = cppFunctions.FirstOrDefault();
            if (firstFunction == null)
                return;
            
            var className = GetClassName(firstFunction);
            var @class = ClassDeclaration(className)
                .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword));

            //foreach (var cppType in cppTypes)
            //{
            //    if (!CanProcess(cppType))
            //        continue;

            //    string nativeName = GetNativeName(cppType);
            //    if (TypeMap.TryGetType(nativeName, out var typeInfo))
            //    {
            //        if (IsSameType(typeInfo, cppType))
            //            continue;

            //        throw new ArgumentException($"Duplicate type: '{nativeName}'.");
            //    }

            //    var managedName = GetManagedName(nativeName);

            //    Console.Write($"  Building type '{managedName}' from '{nativeName}'...");
            //    var type = BuildType(cppType, nativeName, managedName);
            //    if (type != null)
            //    {
            //        WriteLineColored("  Done.", ConsoleColor.Green);
            //    }
            //    else
            //    {
            //        WriteLineColored("  Skipped.", ConsoleColor.Yellow);
            //        continue;
            //    }

            //    BuildDocument(GetRelativeNamespace(cppType), type, managedName);
            //}
            BuildDocument(GetRelativeNamespace(firstFunction), @class, className);
        }

        protected override string GetRelativeNamespace(CppFunction cppElement)
        {
            return $"{base.GetRelativeNamespace(cppElement)}.Internal";
        }

        protected override string GetNativeName(CppFunction cppFunction) => cppFunction.Name;

        private string GetClassName(CppFunction cppFunction)
        {
            var file = Path.GetFileNameWithoutExtension(cppFunction.Span.Start.File);
            var name = GetManagedName(file);
            return name;
        }
    }
}
