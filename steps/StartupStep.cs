using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

using System.ComponentModel;

#pragma warning disable SKEXP0080

namespace SKProcess
{
    //public static class StepFunctions
    //{
    //    public const string GetUserInput = nameof(GetUserInput);
    //    public const string ProcessUserInput = nameof(ProcessUserInput);

    //}

    public class StartStep : KernelProcessStep
    {
        [KernelFunction]
        public async ValueTask ExecuteAsync(KernelProcessStepContext context)
        {
            Console.WriteLine("Welcome to the help ticket system.");

            await context.EmitEventAsync(new() { Id = Events.GetUserInput, Data = null });

        }
    }

}
