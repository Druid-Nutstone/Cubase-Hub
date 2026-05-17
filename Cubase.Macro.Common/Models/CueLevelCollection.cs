using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Models
{
    public class CueLevelCollection : List<Cue>
    {
        public CueLevelCollection() { }

        public bool HaveAtLeastOneChange {  get; set; } = false;

        public string[] GetCueNames()
        {
            return this.Select(c => c.Name).ToArray();
        }

        public void UpdateCueLevel(CueLevelChange cueLevelChange)
        {
            var cue =
                this.FirstOrDefault(
                    x => x.Name == cueLevelChange.CueName);

            if (cue == null)
            {
                cue = new Cue()
                {
                    Name = cueLevelChange.CueName
                };

                this.HaveAtLeastOneChange = true;
                this.Add(cue);
            }

            var cueLevel =
                cue.CueLevels.FirstOrDefault(
                    x => x.TrackIndex ==
                         cueLevelChange.TrackIndex);

            if (cueLevel == null)
            {
                this.HaveAtLeastOneChange = true;
                cueLevel = new CueLevel()
                {
                    TrackName =
                        cueLevelChange.TrackName,

                    TrackIndex =
                        cueLevelChange.TrackIndex
                };

                cue.CueLevels.Add(cueLevel);
            }

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

            cueLevel.Volume = cueLevelChange.CueLevel; 

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

        public int TrackIndex { get; set; } = -1;

        public string TrackName { get; set; }

        public double Volume { get; set; } = 0;

        public bool Enabled { get; set; } 
    }


}
