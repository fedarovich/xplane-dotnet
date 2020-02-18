using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public abstract class TypeBuilderBase<T> : BuilderBase<T> where T : CppType
    {
        protected TypeBuilderBase(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap) : base(workspace, projectId, directory, typeMap)
        {
        }

        public override void Build(CppContainerList<T> cppTypes)
        {
            foreach (var cppType in cppTypes)
            {
                if (!CanProcess(cppType))
                    continue;

                string nativeName = GetNativeName(cppType);
                if (TypeMap.TryGetType(nativeName, out var typeInfo))
                {
                    if (IsSameType(typeInfo, cppType))
                        continue;

                    throw new ArgumentException($"Duplicate type: '{nativeName}'.");
                }

                var managedName = GetManagedName(nativeName);
                var ns = GetRelativeNamespace(cppType);
                if (ns == null)
                    continue;
                

                Console.Write($"  Building type '{managedName}' from '{nativeName}'...");
                var type = BuildType(cppType, nativeName, managedName);
                if (type != null)
                {
                    WriteLineColored("  Done.", ConsoleColor.Green);
                }
                else
                {
                    WriteLineColored("  Skipped.", ConsoleColor.Yellow);
                    continue;
                }
                
                BuildDocument(ns, type, managedName);
            }
        }

        protected virtual bool IsSameType(TypeInfo typeInfo, T cppType) => typeInfo.IsSame(cppType);

        protected abstract MemberDeclarationSyntax BuildType(T cppType, string nativeName, string managedName);
    }
}
