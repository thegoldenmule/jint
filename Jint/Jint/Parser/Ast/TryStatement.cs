using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class TryStatement : Statement
    {
        public Statement Block;
        public List<Statement> GuardedHandlers;
        public List<CatchClause> Handlers;
        public Statement Finalizer;
    }
}