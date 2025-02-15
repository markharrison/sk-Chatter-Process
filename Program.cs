using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Process.Tools;

#pragma warning disable SKEXP0080

namespace SKProcess
{
    internal class Program
    {
        public static class ProcessEvents
        {
            public const string StartProcess = nameof(StartProcess);
        }

        static async Task Main(string[] args)
        {
            await Task.Run(() => { });

            Console.WriteLine("*** SK Process ***");

            AppSettings setx = new();

            var kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.Services.ConfigureHttpClientDefaults(
                c => c.AddStandardResilienceHandler(
                    c2 => {
                        TimeSpan timeSpan = TimeSpan.FromMinutes(2);
                        c2.AttemptTimeout.Timeout = timeSpan;
                        c2.CircuitBreaker.SamplingDuration = timeSpan * 2;
                        c2.TotalRequestTimeout.Timeout = timeSpan * 3;
                    }
                ));
            kernelBuilder.Services.AddLogging(c => c.AddConsole().SetMinimumLevel(LogLevel.Warning));

            kernelBuilder.AddAzureOpenAIChatCompletion(setx.azopenaiCCDeploymentname, setx.azopwnaiEndpoint, setx.azopwnaiApikey);

            var kernel = kernelBuilder.Build();

            var process = SKProcess.Setup("SK Process Demo");

            using var runningProcess = await process.StartAsync(kernel, new KernelProcessEvent()
            {
                Id = ProcessEvents.StartProcess,
                Data = "HELLO WORLD"
            });

            ChatHistory history = [];

            //foreach (var message in history)
            //{
            //    RenderMessageStep.Render(message);
            //}


        }

    }
}
