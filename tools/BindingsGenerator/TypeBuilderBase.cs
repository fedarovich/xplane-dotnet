using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BindingsGenerator
{
    public abstract class TypeBuilderBase<T> : BuilderBase<T> where T : CppType
    {
        protected TypeBuilderBase(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap) : base(workspace, projectId, directory, typeMap)
        {
        }

        public override async Task BuildAsync(IEnumerable<T> cppTypes)
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
                

                Log.WriteLine($"Building type '{managedName}' from '{nativeName}'...");
                var type = BuildType(cppType, nativeName, managedName);
                if (type != null)
                {
                    TypeMap.RegisterType(cppType, nativeName, managedName, GetFullNamespace(cppType), GetFunctionPointerTypeSyntax(cppType));
                    Log.WriteLine("Done.", ConsoleColor.Green);
                }
                else
                {
                    Log.WriteLine("Skipped.", ConsoleColor.Yellow);
                    continue;
                }
                
                await BuildDocumentAsync(ns, type, managedName);
            }
        }

        protected virtual bool IsSameType(TypeInfo typeInfo, T cppType) => typeInfo.IsSame(cppType);

        protected abstract MemberDeclarationSyntax BuildType(T cppType, string nativeName, string managedName);

        protected virtual FunctionPointerTypeSyntax GetFunctionPointerTypeSyntax(CppType cppType)
        {
            return null;
        }
    }
}
