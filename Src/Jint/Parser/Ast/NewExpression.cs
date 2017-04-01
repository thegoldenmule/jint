using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class NewExpression : Expression
    {
        public Expression Callee;
        public List<Expression> Arguments;
    }
}