using Microsoft.SemanticKernel;

#pragma warning disable SKEXP0080

namespace SKProcess
{
    public static class ProcessEvents
    {
        public const string StartProcess = nameof(StartProcess);
    }
    public static class SKProcess
    {
        public static KernelProcess Setup(string processName)
        {
            ProcessBuilder process = new(processName);

            var startStep = process.AddStepFromType<StartStep>();
            var getUserInputStep = process.AddStepFromType<GetUserInputStep>();
            var triageStep = process.AddStepFromType<TriageStep>();
            var securityTicketStep = process.AddStepFromType<SecurityTicketStep>();
            var hardwareTicketStep = process.AddStepFromType<HardwareTicketStep>();
            var softwareTicketStep = process.AddStepFromType<SoftwareTicketStep>();

            process
                .OnInputEvent(ProcessEvents.StartProcess)
                .SendEventTo(new ProcessFunctionTargetBuilder(startStep));

            startStep
                .OnEvent(Events.GetUserInput)
                .SendEventTo(new ProcessFunctionTargetBuilder(getUserInputStep, StepFunctions.GetUserInput));

            getUserInputStep
                .OnEvent(Events.Triage)
                .SendEventTo(new ProcessFunctionTargetBuilder(triageStep, StepFunctions.TriageStep, "input"));

            triageStep
                .OnEvent(Events.GetUserInput)
                .SendEventTo(new ProcessFunctionTargetBuilder(getUserInputStep, StepFunctions.GetUserInput));

            triageStep
                .OnEvent(Events.SecurityTicket)
                .SendEventTo(new ProcessFunctionTargetBuilder(securityTicketStep, StepFunctions.SecurityTicket, "eventdata"));

            triageStep
                .OnEvent(Events.HardwareTicket)
                .SendEventTo(new ProcessFunctionTargetBuilder(hardwareTicketStep, StepFunctions.HardwareTicket, "eventdata"));

            triageStep
                .OnEvent(Events.SoftwareTicket)
                .SendEventTo(new ProcessFunctionTargetBuilder(softwareTicketStep, StepFunctions.SoftwareTicket, "eventdata"));

            triageStep
                .OnFunctionResult()
                .StopProcess();



            KernelProcess kernelProcess = process.Build();

            return kernelProcess;

        }
    }
}
