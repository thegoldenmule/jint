using System;
using System.Collections.Generic;

namespace Jint.Unity
{
    [Serializable]
    public class ScriptingProperty
    {
        public string Name;
        public object Value;
        public Type Type;
    }

    [Serializable]
    public class ScriptingPropertyBucket
    {
        public List<ScriptingProperty> All;
    }
}