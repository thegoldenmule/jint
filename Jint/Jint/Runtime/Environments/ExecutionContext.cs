using Jint.Native;

namespace Jint.Runtime.Environments
{
    public sealed class ExecutionContext
    {
        public LexicalEnvironment LexicalEnvironment;
        public LexicalEnvironment VariableEnvironment;
        public JsValue ThisBinding;
    }
}
