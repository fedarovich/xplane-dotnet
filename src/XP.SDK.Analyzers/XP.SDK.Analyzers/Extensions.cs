using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;

namespace XP.SDK.Analyzers
{
    internal static class Extensions
    {
        internal static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value) => (key, value) = (pair.Key, pair.Value);

        internal static unsafe StringBuilder Append(this StringBuilder builder, in ReadOnlySpan<char> chars)
        {
            fixed (char* p = chars)
            {
                builder.Append(p, chars.Length);
            }

            return builder;
        }

        internal static unsafe string AsString(this in ReadOnlySpan<char> chars)
        {
            fixed (char* p = chars)
            {
                return new string(p, 0, chars.Length);
            }
        }

        internal static void WriteBytes(this TextWriter writer, in ReadOnlySpan<byte> bytes)
        {
            foreach (var b in bytes)
            {
                writer.Write("0x{0:X2}, ", b);
            }
        }

        internal static bool IsSubtypeOf(this INamedTypeSymbol type, INamedTypeSymbol baseType)
        {
            var parent = type;
            do
            {
                if (SymbolEqualityComparer.Default.Equals(parent, baseType))
                    return true;

                parent = parent.BaseType;
            } while (parent != null);

            if (baseType.TypeKind == TypeKind.Interface)
            {
                foreach (var @interface in type.AllInterfaces)
                {
                    if (SymbolEqualityComparer.Default.Equals(@interface, baseType))
                        return true;
                }
            }

            return false;
        }
    }
}
