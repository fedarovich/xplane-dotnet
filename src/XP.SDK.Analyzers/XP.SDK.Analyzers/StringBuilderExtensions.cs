using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Analyzers
{
    internal static class StringBuilderExtensions
    {
        private const string Ident = "    ";

        public static StringBuilder AppendIdent(this StringBuilder builder, int ident)
        {
            for (int i = 0; i < ident; i++)
            {
                builder.Append(Ident);
            }

            return builder;
        }
    }
}
