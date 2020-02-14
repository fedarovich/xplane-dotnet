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
        private readonly Dictionary<string, TypeInfo> _nativeMap = new Dictionary<string, TypeInfo>();
        private readonly Dictionary<string, TypeInfo> _managedMap = new Dictionary<string, TypeInfo>();

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

        public TypeInfo RegisterType(CppType cppType, string nativeName, string managedName)
        {
            var typeInfo = new TypeInfo(nativeName, cppType, IdentifierName(managedName));
            _nativeMap.Add(nativeName, typeInfo);
            _managedMap.Add(managedName, typeInfo);
            return typeInfo;
        }

        private void RegisterPrimitiveType(CppPrimitiveType primitive, SyntaxKind syntax)
        {
            var name = primitive.GetDisplayName();
            var typeInfo = new TypeInfo(name, primitive, PredefinedType(Token(syntax)));
            _nativeMap.Add(name, typeInfo);
        }

        public TypeInfo GetType(string name) => _nativeMap[name];

        public bool TryGetType(string name, out TypeInfo typeInfo) => _nativeMap.TryGetValue(name, out typeInfo);
    }
}
