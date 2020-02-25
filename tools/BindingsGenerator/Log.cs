using System;
using System.Collections.Generic;
using System.Text;

namespace BindingsGenerator
{
    public static class Log
    {
        private static string _ident = string.Empty;

        public static void WriteLine(string text)
        {
            Console.Write(_ident);
            Console.WriteLine(text);
        }

        public static void WriteLine(string text, ConsoleColor color)
        {
            Console.Write(_ident);
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static IDisposable PushIdent()
        {
            var holder = new IdentHolder(_ident);
            _ident += "  ";
            return holder;
        }

        private class IdentHolder : IDisposable
        {
            private readonly string _ident;

            public IdentHolder(string ident)
            {
                _ident = ident;
            }

            public void Dispose()
            {
                Log._ident = _ident;
            }
        }
    }
}
