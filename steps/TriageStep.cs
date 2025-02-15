using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using System.Text.Json;

using System.ComponentModel;

#pragma warning disable SKEXP0080

namespace SKProcess
{
    public static partial class StepFunctions
    {
        public const string TriageStep = nameof(TriageStep);
    }

    public class TriageStep : KernelProcessStep
    {
        [KernelFunction(StepFunctions.TriageStep)]
        public async ValueTask ProcessUserInputAsync(KernelProcessStepContext context, Kernel _kernel, string input)
        {
            Console.WriteLine("Triage...");

            string triageAgentInstruction = $$$"""
            Look at a users problem and triage it .
            Decide if the problem is HARDWARE, SOFTWARE, SECURITY or FACILITIES.   If unsure then classify as UNSURE.
            SECURITY can include permissions / logon / access issues.
            Also summarise the problem as short as possible and always less than 50 characters.
            All responses must start with: HARDWARE, SOFTWARE, SECURITY or UNSURE - and then followed by the summary .
            """;

            // Get the chat history
            var chatService = _kernel.GetRequiredService<IChatCompletionService>();

            ChatHistory chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage(triageAgentInstruction);

            chatHistory.AddUserMessage(input);

            var contents = chatService.GetStreamingChatMessageContentsAsync(
                        chatHistory,
                        new AzureOpenAIPromptExecutionSettings(),
                        _kernel);

            string fullContent = "";
            await foreach (var content in contents)
            {
                Console.Write(content);
                fullContent += content;
            }
            Console.Write("\n");

            if (fullContent.StartsWith("UNSURE"))
            {
                await context.EmitEventAsync(new() { Id = Events.GetUserInput, Data = null });
            }
            else if (fullContent.StartsWith("SECURITY"))
            {
                var data = new { UserInput = input, Triage = fullContent };
                string jsonData = JsonSerializer.Serialize(data);
                await context.EmitEventAsync(Events.SecurityTicket, data: jsonData);
            }
            else if (fullContent.StartsWith("HARDWARE"))
            {
                var data = new { UserInput = input, Triage = fullContent };
                string jsonData = JsonSerializer.Serialize(data);
                await context.EmitEventAsync(Events.HardwareTicket, data: jsonData);
            }
            else if (fullContent.StartsWith("SOFTWARE"))
            {
                var data = new { UserInput = input, Triage = fullContent };
                string jsonData = JsonSerializer.Serialize(data);
                await context.EmitEventAsync(Events.SoftwareTicket, data: jsonData);
            }
            else
            {
                await context.EmitEventAsync(new() { Id = Events.GetUserInput, Data = null });
            }



        }

    }


}
