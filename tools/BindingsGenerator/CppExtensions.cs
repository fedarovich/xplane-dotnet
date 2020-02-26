using System;
using System.Collections.Generic;
using System.Text;
using CppAst;

namespace BindingsGenerator
{
    internal static class CppExtensions
    {
        public static bool IsConstCharPtr(this CppType type) =>
            type is CppPointerType
            {
                ElementType: CppQualifiedType
                {
                    Qualifier: CppTypeQualifier.Const,
                    ElementType: CppPrimitiveType { Kind: CppPrimitiveKind.Char }
                }
            };
    }
}
