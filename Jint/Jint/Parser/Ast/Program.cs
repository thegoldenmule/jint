using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class Program : Statement, IFunctionScope
    {
        public Program()
        {
            VariableDeclarations = new List<VariableDeclaration>();
        }
        public List<Statement> Body;

        public List<Comment> Comments;
        public List<Token> Tokens;
        public List<ParserException> Errors;
        public bool Strict;

        public List<VariableDeclaration> VariableDeclarations { get; set; }
        public List<FunctionDeclaration> FunctionDeclarations { get; set; }
    }
}