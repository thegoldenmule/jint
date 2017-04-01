using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Jint.Runtime.Interop;

namespace Jint
{
    public class Options
    {
        public bool DiscardGlobal;
        public bool Strict;
        public bool AllowDebuggerStatement;
        public int MaxStatements;
        public int MaxRecursionDepth = -1;
        public TimeSpan TimeoutInterval;
        public CultureInfo Culture = CultureInfo.CurrentCulture;
        public TimeZoneInfo LocalTimeZone;
        
        public List<Assembly> LookupAssemblies = new List<Assembly>();
        public readonly List<IObjectConverter> ObjectConverters = new List<IObjectConverter>();

        public bool IsClrAllowed { get; private set; }

        public Options()
        {
            LocalTimeZone = TimeZoneInfo.Local;
        }

        /// <summary>
        /// Allows scripts to call CLR types directly like <example>System.IO.File</example>
        /// </summary>
        public Options AllowClr(params Assembly[] assemblies)
        {
            IsClrAllowed = true;
            LookupAssemblies.AddRange(assemblies);
            LookupAssemblies = LookupAssemblies.Distinct().ToList();
            return this;
        }
    }
}
