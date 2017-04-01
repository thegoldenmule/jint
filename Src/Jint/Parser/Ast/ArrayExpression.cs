using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class ArrayExpression : Expression
    {
        public List<Expression> Elements;
    }
}