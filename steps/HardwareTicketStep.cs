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
        public const string HardwareTicket = nameof(HardwareTicket);

    }

    public class HardwareTicketStep : KernelProcessStep
    {
        [KernelFunction(StepFunctions.HardwareTicket)]
        public async ValueTask HardwareTicketAsync(KernelProcessStepContext context)
        {
            Console.WriteLine("Hardware Ticket");

            await Task.Run(() => { });  

        }

    }
  

}
