using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Models
{
    public class CueLevelCollection : List<Cue>
    {
        private Dictionary<MidiCueMessageType, Action<CueLevelChange, CueLevel>> cueMessageHandlers;
        
        public CueLevelCollection() 
        {
           cueMessageHandlers = new Dictionary<MidiCueMessageType, Action<CueLevelChange, CueLevel>>()
           {
               { MidiCueMessageType.Volume, HandleVolumeChange },
               { MidiCueMessageType.Enable, HandleEnableChange },
               { MidiCueMessageType.Mute, HandleMuteChange },
               { MidiCueMessageType.Solo, HandleSoloChange },
               { MidiCueMessageType.RecordEnable, HandleRecordEnableChange },
               { MidiCueMessageType.Selected, HandleSelectedChange },
               { MidiCueMessageType.TrackType, HandleTrackTypeChange }
           };  
        }

        private void HandleSelectedChange(CueLevelChange cueLevelChange, CueLevel cueLevel)
        {
            if (cueLevel.Selected != cueLevelChange.Selected)
            {
                cueLevel.Selected = cueLevelChange.Selected;
                this.HaveAtLeastOneChange = true;
            }
        }

        private void HandleTrackTypeChange(CueLevelChange cueLevelChange, CueLevel cueLevel)
        {
            if (cueLevel.TrackType != cueLevelChange.TrackType)
            {
                cueLevel.TrackType = cueLevelChange.TrackType;
                this.HaveAtLeastOneChange = true;
            }
        }

        private void HandleVolumeChange(CueLevelChange cueLevelChange, CueLevel cueLevel)
        {
            if (cueLevelChange.CueLevel <= 0)
            {
                if (cueLevelChange.CueMessageType == MidiCueMessageType.Volume && cueLevelChange.CueEnabled == -1)
                {
                    cueLevel.Enabled = false;
                    this.HaveAtLeastOneChange = true;
                }
                if (cueLevelChange.CueMessageType == MidiCueMessageType.Volume && cueLevelChange.CueEnabled == 1)
                {
                    cueLevel.Enabled = true;
                    this.HaveAtLeastOneChange = true;
                }
                return;
            }

            if (cueLevel.Volume != cueLevelChange.CueLevel)
            {
                cueLevel.Volume = cueLevelChange.CueLevel;
                this.HaveAtLeastOneChange = true;
            }

        }

        private void HandleEnableChange(CueLevelChange cueLevelChange, CueLevel cueLevel)
        {
            if (cueLevel.Enabled != cueLevelChange.CueEnabled > 0)
            {
                cueLevel.Enabled = cueLevelChange.CueEnabled > 0;
                this.HaveAtLeastOneChange = true;
            }
        }

        private void HandleMuteChange(CueLevelChange cueLevelChange, CueLevel cueLevel)
        {
            if (cueLevel.Mute != cueLevelChange.Mute)
            {
                cueLevel.Mute = cueLevelChange.Mute;
                this.HaveAtLeastOneChange = true;
            }
        }

        private void HandleSoloChange(CueLevelChange cueLevelChange, CueLevel cueLevel)
        {
            if (cueLevel.Solo != cueLevelChange.Solo)
            {
                cueLevel.Solo = cueLevelChange.Solo;
                this.HaveAtLeastOneChange = true;
            }
        }

        private void HandleRecordEnableChange(CueLevelChange cueLevelChange, CueLevel cueLevel)
        {
            if (cueLevel.RecordEnable != cueLevelChange.RecordEnable)
            {
                cueLevel.RecordEnable = cueLevelChange.RecordEnable;
                this.HaveAtLeastOneChange = true;
            }
        }

        private CueLevel GetOrCreateCueLevel(CueLevelChange cueLevelChange)
        {
            var cue = this.FirstOrDefault(x => x.Name == cueLevelChange.CueName);

            if (cue == null)
            {
                cue = new Cue()
                {
                    Name = cueLevelChange.CueName
                };

                this.HaveAtLeastOneChange = true;
                this.Add(cue);
            }

            var cueLevel = cue.CueLevels.FirstOrDefault(x => x.Id == cueLevelChange.Id);

            if (cueLevel == null)
            {
                this.HaveAtLeastOneChange = true;
                cueLevel = new CueLevel()
                {
                    TrackName = cueLevelChange.TrackName,
                    TrackIndex = cueLevelChange.TrackIndex,
                    Id = cueLevelChange.Id,
                    Mute = cueLevelChange.Mute,
                    Solo = cueLevelChange.Solo,
                    Volume = cueLevelChange.CueLevel,
                    Selected = cueLevelChange.Selected,
                    TrackType = cueLevelChange.TrackType,
                    RecordEnable = cueLevelChange.RecordEnable
                };
                cue.CueLevels.Add(cueLevel);
            }
            return cueLevel;
        }

        public bool HaveAtLeastOneChange {  get; set; } = false;

        public string[] GetCueNames()
        {
            return this.Select(c => c.Name).ToArray();
        }

        public void UpdateCueLevel(CueLevelChange cueLevelChange)
        {
            if (this.cueMessageHandlers.ContainsKey(cueLevelChange.CueMessageType))
            {
                this.cueMessageHandlers[cueLevelChange.CueMessageType](cueLevelChange, this.GetOrCreateCueLevel(cueLevelChange));
                return;
            }

            var cueLevel = this.GetOrCreateCueLevel(cueLevelChange);

            // cubase will send 0 or < 0 values when updating the enabled state
            // OR a volume update. since the volume cannot be < 0 igore the update from cubase 
            if (cueLevelChange.CueLevel <= 0)
            {
                if (cueLevelChange.CueMessageType == MidiCueMessageType.Volume && cueLevelChange.CueEnabled == -1)
                {
                    cueLevel.Enabled = false;
                    this.HaveAtLeastOneChange = true;
                }
                if (cueLevelChange.CueMessageType == MidiCueMessageType.Volume && cueLevelChange.CueEnabled == 1)
                {
                    cueLevel.Enabled = true;
                    this.HaveAtLeastOneChange = true;
                }
                return;
            }
            if (cueLevel.Mute != cueLevelChange.Mute || cueLevel.Solo != cueLevelChange.Solo || cueLevel.RecordEnable != cueLevelChange.RecordEnable)
            {
                this.HaveAtLeastOneChange = true;
            }
            cueLevel.Id = cueLevelChange.Id;
            cueLevel.Volume = cueLevelChange.CueLevel; 
            cueLevel.TrackIndex = cueLevelChange.TrackIndex;
            cueLevel.Mute = cueLevelChange.Mute;
            cueLevel.Solo = cueLevelChange.Solo;
            cueLevel.RecordEnable = cueLevelChange.RecordEnable;
            cueLevel.Selected = cueLevelChange.Selected;
            cueLevel.TrackType = cueLevelChange.TrackType;
            if (cueLevelChange.CueEnabled > -1)
            {
                cueLevel.Enabled =
                    cueLevelChange.CueEnabled > 0;  
            }
        }
    }

    public class Cue
    {
        public string Name { get; set; }

        public List<CueLevel> CueLevels { get; set; } = new List<CueLevel>();
    }

    public class CueLevel
    {
        public string Id { get; set; }
        
        public int TrackIndex { get; set; }

        public string TrackName { get; set; }

        public double Volume { get; set; } = 0;

        public bool Enabled { get; set; } 

        public bool Mute { get; set; }

        public bool Solo { get; set; }


        public bool RecordEnable { get; set; }

        public bool Selected { get; set; }

        public string TrackType { get; set; }
    }


}
