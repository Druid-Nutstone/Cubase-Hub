using Cubase.Macro.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Cubase.Macro.Services.ConditionalMacros
{
    public class ConditionalMacroService : IConditionalMacroService
    {
        private readonly IServiceProvider serviceProvider;

        public ConditionalMacroService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public CubaseKeyCommand? ExecuteConditionalMacro(CubaseKeyCommand cubaseKeyCommand, Action<string> onError)
        {
            var availableFunctions = this.serviceProvider.GetServices<IConditionalMacroFunction>();

            var result = cubaseKeyCommand.Conditional.Command;
            foreach (var condition in cubaseKeyCommand.Conditional.Conditions)
            {
                var functionProcessor = availableFunctions.FirstOrDefault(x => x.FunctionName == condition.FunctionName);
                if (functionProcessor == null)
                {
                    onError.Invoke($"Could not find function {condition.FunctionName}");
                    return null;
                }
                var targetResult = functionProcessor.GetPropertyState(condition.PropertyName);
                if (targetResult != condition.State)
                {
                    onError.Invoke($"Could NOT invoke midi command. {condition.FunctionName} with property {condition.PropertyName} was {targetResult}. but the macro expected {condition.State}");
                    return null;
                }
            }
            return result;
        }

        public List<string> GetFunctionNames()
        {
            var allFunctionServices = this.serviceProvider.GetServices<IConditionalMacroFunction>();
            return allFunctionServices.Select(x => x.FunctionName).ToList();
        }

        public List<string> GetFunctionProperties(string functionName)
        {
            var function = this.serviceProvider.GetServices<IConditionalMacroFunction>().FirstOrDefault(x => x.FunctionName == functionName);
            if (function != null)
            {
                return function.Properties.ToList();
            }
            return new List<string>();
        }

        public void Initialise()
        {
            var allFunctionServices = this.serviceProvider.GetServices<IConditionalMacroFunction>();
            foreach (var item in allFunctionServices)
            {
                item.Initialise();
            }
        }

    }
}
