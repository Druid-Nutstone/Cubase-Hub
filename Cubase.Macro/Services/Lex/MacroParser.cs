
using Lex.Tokens;
using System.Globalization;
namespace Cubase.Macro.Services.Lex
{
    public class MacroParser
    {
        private readonly List<Token> _tokens;
        private int _position;

        public MacroParser(IEnumerable<Token> tokens)
        {
            // Ignore whitespace and comments
            _tokens = tokens
                .Where(t => t is not WhitespaceToken &&
                            t is not CommentToken)
                .ToList();
        }

        private Token Current =>
            _position < _tokens.Count
                ? _tokens[_position]
                : null;

        private Token Peek(int offset = 1)
        {
            int index = _position + offset;

            return index < _tokens.Count
                ? _tokens[index]
                : null;
        }

        private Token Advance()
        {
            Token token = Current;

            if (token != null)
                _position++;

            return token;
        }

        private bool Is(string value)
        {
            return Current != null &&
                   Current.Text.Equals(
                       value,
                       StringComparison.OrdinalIgnoreCase);
        }

        private bool Match(string value)
        {
            if (!Is(value))
                return false;

            Advance();
            return true;
        }

        private Token Expect(string value)
        {
            if (!Is(value))
                throw Error($"Expected '{value}'.");

            return Advance();
        }

        private Exception Error(string message)
        {
            if (Current == null)
                return new Exception(
                    $"Line end: {message}");

            return new Exception(
                $"Line {Current.Line}, Column {Current.Column}: {message}");
        }

        // ==========================================
        // PROGRAM
        // ==========================================

        public List<Statement> Parse()
        {
            var statements = new List<Statement>();

            while (Current != null)
            {
                statements.Add(ParseStatement());
            }

            return statements;
        }

        // ==========================================
        // STATEMENTS
        // ==========================================

        private Statement ParseStatement()
        {
            // Variable declaration
            if (Match("var"))
                return ParseVariableDeclaration();

            // Conditional
            if (Match("if"))
                return ParseIfStatement();

            // Assignment
            if (Current is IdToken && Peek()?.Text == "=")
                return ParseAssignment();

            // Function call / expression
            return new ExpressionStatement
            {
                Expression = ParseExpression()
            };
        }

        private Statement ParseVariableDeclaration()
        {
            if (Current is not IdToken)
                throw Error("Expected variable name.");

            string name = Advance().Text;

            Expect("=");

            return new VariableDeclaration
            {
                Name = name,
                Value = ParseExpression()
            };
        }

        private Statement ParseAssignment()
        {
            string name = Advance().Text;

            Expect("=");

            return new AssignmentStatement
            {
                Name = name,
                Value = ParseExpression()
            };
        }

        // ==========================================
        // IF / THEN / ELSE / END IF
        // ==========================================

        private IfStatement ParseIfStatement()
        {
            var statement = new IfStatement();

            statement.Condition = ParseExpression();

            Expect("then");

            // THEN block
            while (Current != null &&
                   !Is("else") &&
                   !Is("end"))
            {
                statement.ThenStatements.Add(
                    ParseStatement());
            }

            // ELSE block
            if (Match("else"))
            {
                while (Current != null && !Is("end"))
                {
                    statement.ElseStatements.Add(
                        ParseStatement());
                }
            }

            Expect("end");
            Expect("if");

            return statement;
        }

        // ==========================================
        // EXPRESSIONS
        // ==========================================

        private Expression ParseExpression()
        {
            Expression left = ParsePrimary();

            // Comparisons
            if (Current is OperatorToken &&
                IsComparisonOperator(Current.Text))
            {
                string op = Advance().Text;

                Expression right = ParsePrimary();

                return new BinaryExpression
                {
                    Left = left,
                    Operator = op,
                    Right = right
                };
            }

            return left;
        }

        private bool IsComparisonOperator(string op)
        {
            return op == "==" ||
                   op == "!=" ||
                   op == ">" ||
                   op == "<" ||
                   op == ">=" ||
                   op == "<=";
        }

        private Expression ParsePrimary()
        {
            if (Current == null)
                throw Error("Unexpected end of expression.");

            // Parenthesised expression
            if (Match("("))
            {
                Expression expression = ParseExpression();

                Expect(")");

                return expression;
            }

            // String
            if (Current is StringToken)
            {
                string value = Advance().Text;

                // Remove surrounding quotes if Lex retains them
                if (value.Length >= 2 &&
                    ((value[0] == '"' && value[^1] == '"') ||
                     (value[0] == '\'' && value[^1] == '\'')))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                return new LiteralExpression
                {
                    Value = value
                };
            }

            // Number
            if (Current is NumberToken)
            {
                string value = Advance().Text;

                if (!double.TryParse(
                        value,
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out double number))
                {
                    throw Error($"Invalid number '{value}'.");
                }

                return new LiteralExpression
                {
                    Value = number
                };
            }

            // Keywords representing literal values
            if (Current is KeywordToken)
            {
                if (Match("null"))
                    return new LiteralExpression { Value = null };

                if (Match("true"))
                    return new LiteralExpression { Value = true };

                if (Match("false"))
                    return new LiteralExpression { Value = false };
            }

            // Identifier: function or variable
            if (Current is IdToken)
            {
                string name = Advance().Text;

                if (Match("("))
                    return ParseFunctionCall(name);

                return new VariableExpression
                {
                    Name = name
                };
            }

            throw Error("Unexpected token in expression.");
        }

        // ==========================================
        // FUNCTION CALLS
        // ==========================================

        private Expression ParseFunctionCall(string name)
        {
            var call = new FunctionCallExpression
            {
                Name = name
            };

            // Empty argument list
            if (Match(")"))
                return call;

            do
            {
                call.Arguments.Add(ParseExpression());

            } while (Match(","));

            Expect(")");

            return call;
        }
    }
}
