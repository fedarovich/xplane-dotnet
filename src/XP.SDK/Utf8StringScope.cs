#nullable enable
using System;

namespace XP.SDK
{
    public readonly ref struct Utf8StringScope
    {
        public Utf8String String { get; }

        private readonly IDisposable? _disposable;

        public Utf8StringScope(in Utf8String @string, IDisposable? disposable)
        {
            String = @string;
            _disposable = disposable;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
