using System;
using System.Collections.Generic;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public class TypeMap
    {
        private readonly Dictionary<string, TypeInfo> _map = new Dictionary<string, TypeInfo>();

        public TypeMap()
        {
            RegisterPrimitiveType(CppPrimitiveType.Void, SyntaxKind.VoidKeyword);
            RegisterPrimitiveType(CppPrimitiveType.Char, SyntaxKind.ByteKeyword);
            RegisterPrimitiveType(CppPrimitiveType.Short, SyntaxKind.ShortKeyword);
            RegisterPrimitiveType(CppPrimitiveType.Int, SyntaxKind.IntKeyword);
            RegisterPrimitiveType(CppPrimitiveType.LongLong, SyntaxKind.LongKeyword);
            RegisterPrimitiveType(CppPrimitiveType.UnsignedChar, SyntaxKind.ByteKeyword);
            RegisterPrimitiveType(CppPrimitiveType.UnsignedShort, SyntaxKind.UShortKeyword);
            RegisterPrimitiveType(CppPrimitiveType.UnsignedInt, SyntaxKind.UIntKeyword);
            RegisterPrimitiveType(CppPrimitiveType.UnsignedLongLong, SyntaxKind.ULongKeyword);
            RegisterPrimitiveType(CppPrimitiveType.Float, SyntaxKind.FloatKeyword);
            RegisterPrimitiveType(CppPrimitiveType.Double, SyntaxKind.DoubleKeyword);
            RegisterPrimitiveType(CppPrimitiveType.WChar, SyntaxKind.CharKeyword);
            RegisterPrimitiveType(CppPrimitiveType.Bool, SyntaxKind.BoolKeyword);
        }

        public TypeInfo RegisterEnumType(string name, CppEnum @enum, string enumName)
        {
            var typeInfo = new TypeInfo(name, @enum, IdentifierName(enumName));
            _map.Add(name, typeInfo);
            return typeInfo;
        }

        private void RegisterPrimitiveType(CppPrimitiveType primitive, SyntaxKind syntax)
        {
            var name = primitive.GetDisplayName();
            var typeInfo = new TypeInfo(name, primitive, PredefinedType(Token(SyntaxKind.IntKeyword)));
            _map.Add(name, typeInfo);
        }

        public bool TryGetType(string name, out TypeInfo typeInfo)
        {
            return _map.TryGetValue(name, out typeInfo);
        }
    }
}
