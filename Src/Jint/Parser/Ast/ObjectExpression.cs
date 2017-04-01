using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class ObjectExpression : Expression
    {
        public List<Property> Properties;
    }
}