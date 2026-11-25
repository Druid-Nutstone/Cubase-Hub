namespace Cubase.Macro.Services.Lex
{

    public abstract class Statement
    {
    }

    public abstract class Expression
    {
    }

    // var track = ...
    public class VariableDeclaration : Statement
    {
        public string Name { get; set; }
        public Expression Value { get; set; }
    }

    // track = ...
    public class AssignmentStatement : Statement
    {
        public string Name { get; set; }
        public Expression Value { get; set; }
    }

    // if ... then ... else ... end if
    public class IfStatement : Statement
    {
        public Expression Condition { get; set; }

        public List<Statement> ThenStatements { get; set; } = new();

        public List<Statement> ElseStatements { get; set; } = new();
    }

    // Function call used as a statement
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; set; }
    }

    // Literal: "Vocal", 123, true, null
    public class LiteralExpression : Expression
    {
        public object Value { get; set; }
    }

    // Variable reference: track
    public class VariableExpression : Expression
    {
        public string Name { get; set; }
    }

    // Function call: selecttrack("Vocal")
    public class FunctionCallExpression : Expression
    {
        public string Name { get; set; }

        public List<Expression> Arguments { get; set; } = new();

    }

    // Comparison: track != null
    public class BinaryExpression : Expression
    {
        public Expression Left { get; set; }

        public string Operator { get; set; }

        public Expression Right { get; set; }
    }

}
