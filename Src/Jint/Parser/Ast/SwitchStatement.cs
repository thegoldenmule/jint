using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class SwitchStatement : Statement
    {
        public Expression Discriminant;
        public List<SwitchCase> Cases;
    }
}