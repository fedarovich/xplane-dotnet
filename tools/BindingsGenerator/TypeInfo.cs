using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BindingsGenerator
{
    public class TypeInfo
    {
        public TypeInfo(string name, CppType cppType, TypeSyntax typeSyntax, bool isStandard = false)
        {
            Name = name;
            CppType = cppType;
            TypeSyntax = typeSyntax;
            IsStandard = isStandard;
        }

        public string Name { get; }

        public CppType CppType { get; }
        
        public TypeSyntax TypeSyntax { get; }

        public bool IsStandard { get; }

        public bool IsPrimitive => CppType is CppPrimitiveType;

        public bool IsEnum => CppType is CppEnum;

        public bool IsVoid => CppType is CppPrimitiveType { Kind: CppPrimitiveKind.Void };

        public bool IsFunction
        {
            get
            {
                return IsFunctionRec(CppType);

                static bool IsFunctionRec(CppType cppType) =>
                    cppType switch
                    {
                        CppFunctionType _ => true,
                        CppPointerType p => IsFunctionRec(p.ElementType),
                        CppTypedef t => IsFunctionRec(t.ElementType),
                        _ => false
                    };
            }
        }

        public bool IsSame(CppType type)
        {
            if (IsStandard && type.GetDisplayName() == Name)
                return true;

            return Path.GetFullPath(CppType.Span.Start.File) == Path.GetFullPath(type.Span.Start.File)
                   && CppType.Span.Start.Offset == type.Span.Start.Offset;
        }
    }
}
