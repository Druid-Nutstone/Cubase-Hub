using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Services.Lex
{
    public class MacroIntepreter : IMacroIntepreter
    {
        private LexVariableCollection variables = new LexVariableCollection();

        private readonly IScriptFunctionService functionService;

        private Action<string> OnError;

        private Action<string> OnProgress;

        public MacroIntepreter(IScriptFunctionService functionService)
        {
            this.functionService = functionService;
        }

        public void ProcessStatements(List<Statement> statements, Action<string> onError, Action<string> onProgress)
        {
            this.variables = new LexVariableCollection();

            this.OnError = onError;

            this.OnProgress = onProgress;


            this.ProcessStatementList(statements);
            OnProgress("Script Complete.");
        }


        private void ProcessVariable(VariableDeclaration variable)
        {
            // add variable name 
            var newVariable = this.variables.AddVariable(variable.Name);
            switch (variable.Value)
            {
                case FunctionCallExpression fce:
                    this.ProcessFunctionCallExpression(fce, newVariable);
                    break;
            }
        }

        private void ProcessFunctionCallExpression(FunctionCallExpression functionCall, LexVariable? variable = null)
        {
            var result = this.functionService.ExecuteFunction(functionCall, this.OnError, this.OnProgress, variable);
        }

        private void ProcessStatementList(List<Statement> statements)
        {
            foreach (var statement in statements)
            {
                switch (statement)
                {
                    case VariableDeclaration varDecl:
                        ProcessVariable(varDecl);
                        break;
                    case ExpressionStatement varExpr:
                        switch (varExpr.Expression)
                        {
                            case FunctionCallExpression varFCE:
                                ProcessFunctionCallExpression(varFCE);
                                break;
                        }

                        break;
                    case IfStatement varIf:
                        ProcessIfStatement(varIf);
                        break;
                    case null:
                        this.OnError("Statement is null in macro");
                        break;
                    default:
                        this.OnError($"Unsupported statement: {statement.GetType().Name}");
                        break;
                }
            }
        }

        private void ProcessIfStatement(IfStatement ifStatement)
        {
            bool shouldContinue = true;
            switch (ifStatement.Condition)
            {
                case BinaryExpression binExpr:
                    shouldContinue = ProcessBinaryExpression(binExpr);
                    break;
            }
            if (shouldContinue)
            {
                this.ProcessStatementList(ifStatement.ThenStatements);
            }
            else
            {
                this.ProcessStatementList(ifStatement.ElseStatements);
            }
        }

        private bool ProcessBinaryExpression(BinaryExpression binaryExpression)
        {
            // var left = binaryExpression.Left

            object? left = null;
            object? right = null;
            switch (binaryExpression.Left)
            {
                case VariableExpression varExpr:
                    left = ProcessVariableExpression(varExpr);
                    break;
            }

            switch (binaryExpression.Right)
            {
                case LiteralExpression liExpr:
                    right = liExpr.Value;
                    break;
            }

            switch (binaryExpression.Operator)
            {
                case "==":
                    return Equals(left, right);

                case "!=":
                    return !Equals(left, right);
                default:
                    return false;
            }
        }

        private object? ProcessVariableExpression(VariableExpression variableExpression)
        {
            var lexVar = this.variables.GetVariableByName(variableExpression.Name);
            return lexVar?.Value;
        }
    }
}
