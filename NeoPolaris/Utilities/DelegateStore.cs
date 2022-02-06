using System;
using System.Collections.Generic;

namespace NeoPolaris.Utilities
{
    /// <summary>
    /// Prevents the .NET GC from stealing our (native) delegates.
    /// </summary>
    internal static class DelegateStore
    {
        private static HashSet<Delegate> _delegates = new();

        public static void Add(Delegate value)
            => _delegates.Add(value);
    }
}
