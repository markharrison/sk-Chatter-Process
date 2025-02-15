using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

using System.ComponentModel;
using System.Text.Json;

#pragma warning disable SKEXP0080

namespace SKProcess
{
    public static partial class StepFunctions
    {
        public const string SecurityTicket = nameof(SecurityTicket);

    }

    public class SecurityTicketStep : KernelProcessStep
    {
        [KernelFunction(StepFunctions.SecurityTicket)]
        public async ValueTask SecurityTicketAsync(KernelProcessStepContext context, Kernel _kernel, string eventdata)
        {
            Console.WriteLine("Security Ticket");

            string securityAgentInstruction = $$$"""
            Is this High Risk or Low Risk ?
            Just response with one word: HIGH, MEDIUM or LOW 
            """;

            var eventDataJson = JsonDocument.Parse(eventdata);
            var userInput = eventDataJson.RootElement.GetProperty("UserInput").GetString();

            var chatService = _kernel.GetRequiredService<IChatCompletionService>();

            ChatHistory chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage(securityAgentInstruction);

            chatHistory.AddUserMessage(userInput!);

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

            await Task.Run(() => { });  

        }

    }
  

}
