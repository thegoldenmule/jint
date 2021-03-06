using System.Collections.Generic;
using Jint.Parser.Ast;

namespace Jint.Parser
{
    /// <summary>
    /// Used to safe references to all variable delcarations in a specific scope.
    /// Hoisting.
    /// </summary>
    public interface IVariableScope
    {
        List<VariableDeclaration> VariableDeclarations { get; set; }
    }

    public class VariableScope : IVariableScope
    {
        public VariableScope()
        {
            VariableDeclarations = new List<VariableDeclaration>();
        }

        public List<VariableDeclaration> VariableDeclarations { get; set; }
    }
}