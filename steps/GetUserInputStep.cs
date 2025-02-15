using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

using System.ComponentModel;

#pragma warning disable SKEXP0080

namespace SKProcess
{
    public static partial class StepFunctions
    {
        public const string GetUserInput = nameof(GetUserInput);
        //public const string ProcessUserInput = nameof(ProcessUserInput);

    }

    public class GetUserInputStep : KernelProcessStep
    {
        [KernelFunction(StepFunctions.GetUserInput)]
        public async ValueTask GetUserInputAsync(KernelProcessStepContext context)
        {

            Console.Write("Please explain your problem: ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Environment.Exit(0);
                return;
            }

            await context.EmitEventAsync(new() { Id = Events.Triage, Data = input });
        }

    }
  

}
