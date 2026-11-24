using Cubase.Macro.Services.Midi;

namespace Cubase.Macro.Services.ConditionalMacros.Transport
{
    public class TransportMacroFunction : IConditionalMacroFunction
    {
        public string FunctionName => "Transport";

        private Dictionary<string, Func<bool>> functions;

        private readonly IMidiService midiService;

        public TransportMacroFunction(IMidiService midiService)
        {
            this.midiService = midiService;
            this.functions = new Dictionary<string, Func<bool>>()
            {
                { "Play", this.ProcessPlay },
                { "Stop", this.ProcessStop },
                { "Record", this.ProcessRecord },
                { "Cycle", this.ProcessCycle },
                { "PunchIn", this.ProcessPunchIn },
                { "PunchOut", this.ProcessPunchOut }
            };
        }

        public IEnumerable<string> Properties => this.functions.Select(x => x.Key);

        public bool GetPropertyState(string propertyName)
        {
            if (this.functions.ContainsKey(propertyName))
            {
                return this.functions[propertyName]();
            }
            return false;
        }

        public void Initialise()
        {
            // nop - nothing to initialise
        }

        public bool ProcessPlay()
        {
            return this.midiService.TransportModel.Play;
        }

        public bool ProcessStop()
        {
            return this.midiService.TransportModel.Stop;
        }

        public bool ProcessCycle()
        {
            return this.midiService.TransportModel.Cycle;
        }

        public bool ProcessRecord()
        {
            return this.midiService.TransportModel.Record;
        }

        public bool ProcessPunchIn()
        {
            return this.midiService.TransportModel.PunchIn;
        }

        public bool ProcessPunchOut()
        {
            return this.midiService.TransportModel.PunchOut;
        }
    }
}
