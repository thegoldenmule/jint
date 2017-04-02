using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class VariableDeclaration : Statement
    {
        public List<VariableDeclarator> Declarations;
        public string Kind;
    }
}