using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Midi;
using Microsoft.Extensions.DependencyInjection;

namespace Cubase.Macro.Services.SysEx
{
    public class SysExService : ISysExService
    {
        private readonly IServiceProvider serviceProvider;

        private IMidiService midiService;

        private Dictionary<string, Func<bool>> functions;

        public IEnumerable<string> SysExCommands => functions.Select(x => x.Key);

        public SysExService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.Initialise();
            this.midiService = this.serviceProvider.GetService<IMidiService>();
        }

        public SysExService()
        {
            this.Initialise();
        }

        private void Initialise()
        {
            this.functions = new Dictionary<string, Func<bool>>()
            {
                {"ClearRecordEnabled", this.ClearEnableRecord },
                {"RecordEnableSelected", this.RecordEnableSelected }
            };
        }

        private bool RecordEnableSelected()
        {
            foreach (var track in this.midiService.TrackCollection.Where(x => x.Selected))
            {
                this.midiService.SendSysExMessage<UpdateTrack>(Models.MidiCommand.UpdateTrack,
                    new UpdateTrack()
                    {
                        Id = track.Id,
                        Value = 1,
                        TrackValueType = TrackUpdateValueType.Record
                    });
            }
            return true;
        }

        private bool ClearEnableRecord()
        {
            foreach (var track in this.midiService.TrackCollection)
            {
                this.midiService.SendSysExMessage<UpdateTrack>(Models.MidiCommand.UpdateTrack,
                    new UpdateTrack()
                    {
                        Id = track.Id,
                        Value = 0,
                        TrackValueType = TrackUpdateValueType.Record
                    });
            }
            return true;
        }

        public bool ExecuteSysExCommand(string commandName, Action<string> onError)
        {
            if (this.functions.ContainsKey(commandName))
            {
                return this.functions[commandName]();
            }
            else
            {
                onError($"Cannot execute {commandName}");
                return false;
            }
        }
    }
}
