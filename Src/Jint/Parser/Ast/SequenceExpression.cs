using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class SequenceExpression : Expression
    {
        public List<Expression> Expressions;
    }
}