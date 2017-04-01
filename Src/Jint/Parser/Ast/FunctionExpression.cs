using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class FunctionExpression : Expression, IFunctionDeclaration
    {
        public FunctionExpression()
        {
            VariableDeclarations = new List<VariableDeclaration>();
        }

        public Identifier Id { get; set; }
        public List<Identifier> Parameters { get; set; }
        public Statement Body { get; set; }
        public bool Strict { get; set; }

        public List<VariableDeclaration> VariableDeclarations { get; set; }
        public List<FunctionDeclaration> FunctionDeclarations { get; set; }

        #region ECMA6
        public IEnumerable<Expression> Defaults;
        public SyntaxNode Rest;
        public bool Generator;
        public bool Expression;
        #endregion

    }
}