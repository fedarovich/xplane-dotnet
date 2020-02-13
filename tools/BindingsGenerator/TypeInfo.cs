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
        public TypeInfo(string name, CppType cppType, TypeSyntax typeSyntax)
        {
            Name = name;
            CppType = cppType;
        }

        public string Name { get; }

        public CppType CppType { get; }

        public bool IsPrimitive => CppType is CppPrimitiveType;

        public bool IsEnum => CppType is CppEnum;

        public bool IsSame(CppType type)
        {
            return Path.GetFullPath(CppType.Span.Start.File) == Path.GetFullPath(type.Span.Start.File)
                   && CppType.Span.Start.Offset == type.Span.Start.Offset;
        }
    }
}
