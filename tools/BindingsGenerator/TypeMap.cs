using System;
using System.Collections.Generic;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public class TypeMap
    {
        private readonly Action<dynamic> _buildType;
        private readonly Dictionary<string, TypeInfo> _nativeMap = new Dictionary<string, TypeInfo>();
        private readonly Dictionary<string, TypeInfo> _managedMap = new Dictionary<string, TypeInfo>();

        public TypeMap(Action<dynamic> buildType)
        {
            _buildType = buildType;
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

            RegisterStandardTypedef("int8_t", CppPrimitiveType.Char, SyntaxKind.SByteKeyword);
            RegisterStandardTypedef("int16_t", CppPrimitiveType.Short, SyntaxKind.ShortKeyword);
            RegisterStandardTypedef("int32_t", CppPrimitiveType.Int, SyntaxKind.IntKeyword);
            RegisterStandardTypedef("int64_t", CppPrimitiveType.LongLong, SyntaxKind.LongKeyword);
            RegisterStandardTypedef("intptr_t", CppPrimitiveType.LongLong, "IntPtr");

            RegisterStandardTypedef("uint8_t", CppPrimitiveType.UnsignedChar, SyntaxKind.ByteKeyword);
            RegisterStandardTypedef("uint16_t", CppPrimitiveType.UnsignedShort, SyntaxKind.UShortKeyword);
            RegisterStandardTypedef("uint32_t", CppPrimitiveType.UnsignedInt, SyntaxKind.UIntKeyword);
            RegisterStandardTypedef("uint64_t", CppPrimitiveType.UnsignedLongLong, SyntaxKind.ULongKeyword);
            RegisterStandardTypedef("uintptr_t", CppPrimitiveType.UnsignedLongLong, "UIntPtr");

            RegisterStandardTypedef("ptrdiff_t", CppPrimitiveType.LongLong, "IntPtr");
            RegisterStandardTypedef("size_t", CppPrimitiveType.UnsignedLongLong, "UIntPtr");
        }

        public TypeInfo RegisterType(CppType cppType, string nativeName, string managedName, string @namespace)
        {
            var typeSyntax = IdentifierName(managedName)
                .WithAdditionalAnnotations(new SyntaxAnnotation(Annotations.Namespace, @namespace));
            var typeInfo = new TypeInfo(nativeName, cppType, typeSyntax);
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

        private void RegisterStandardTypedef(string nativeName, CppPrimitiveType primitive, string managedName)
        {
            var typeInfo = new TypeInfo(nativeName, new CppTypedef(nativeName, primitive), IdentifierName(managedName), true);
            _nativeMap.Add(nativeName, typeInfo);
        }

        private void RegisterStandardTypedef(string nativeName, CppPrimitiveType primitive, SyntaxKind syntax)
        {
            var typeInfo = new TypeInfo(nativeName, new CppTypedef(nativeName, primitive), PredefinedType(Token(syntax)), true);
            _nativeMap.Add(nativeName, typeInfo);
        }

        public TypeInfo GetType(string name) => _nativeMap[name];

        public bool TryGetType(string name, out TypeInfo typeInfo) => _nativeMap.TryGetValue(name, out typeInfo);

        public bool TryResolveType(CppType cppType, out TypeInfo typeInfo, bool lookForward = true)
        {
            if (TryGetType(cppType.GetDisplayName(), out typeInfo))
                return true;

            if (cppType is CppQualifiedType qualified)
            {
                return TryResolveType(qualified.ElementType, out typeInfo, lookForward);
            }

            if (cppType is CppPointerType pointer)
            {
                if (TryResolveType(pointer.ElementType, out var innerTypeInfo, lookForward))
                {
                    typeInfo = new TypeInfo(cppType.GetDisplayName(), cppType, PointerType(innerTypeInfo.TypeSyntax));
                    _nativeMap.Add(typeInfo.Name, typeInfo);
                    return true;
                }
            }

            if (lookForward)
            {
                _buildType(cppType);
                return TryResolveType(cppType, out typeInfo, false);
            }

            return false;
        }
    }
}
