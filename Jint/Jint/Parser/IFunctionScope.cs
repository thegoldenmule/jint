using System.Collections.Generic;
using Jint.Parser.Ast;

namespace Jint.Parser
{
    /// <summary>
    /// Used to safe references to all function delcarations in a specific scope.
    /// </summary>
    public interface IFunctionScope: IVariableScope
    {
        List<FunctionDeclaration> FunctionDeclarations { get; set; }
    }

    public class FunctionScope : IFunctionScope
    {
        public FunctionScope()
        {
            FunctionDeclarations = new List<FunctionDeclaration>();
        }

        public List<FunctionDeclaration> FunctionDeclarations { get; set; }
        public List<VariableDeclaration> VariableDeclarations { get; set; }
    }
}