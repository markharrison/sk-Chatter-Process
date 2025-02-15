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
        public const string SoftwareTicket = nameof(SoftwareTicket);

    }

    public class SoftwareTicketStep : KernelProcessStep
    {
        [KernelFunction(StepFunctions.SoftwareTicket)]
        public async ValueTask SoftwareTicketAsync(KernelProcessStepContext context)
        {
            Console.WriteLine("Software Ticket ");

            await Task.Run(() => { });  

        }

    }
  

}
