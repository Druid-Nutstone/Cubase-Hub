using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Services.Lex
{
    public static class LexExtensions
    {
        public static TrackUpdateValueType ToTrackValueType(this string valueType)
        {
            if (valueType.ToLower().StartsWith("record"))
            {
                return TrackUpdateValueType.Record;
            }
            if (valueType.ToLower().StartsWith("select"))
            {
                return TrackUpdateValueType.Selected;
            }
            if (valueType.ToLower().StartsWith("solo"))
            {
                return TrackUpdateValueType.Solo;
            }
            if (valueType.ToLower().StartsWith("solo"))
            {
                return TrackUpdateValueType.Solo;
            }
            if (valueType.ToLower().StartsWith("mute"))
            {
                return TrackUpdateValueType.Mute;
            }
            return TrackUpdateValueType.Mute;
        }

        public static TransportUpdateValueType ToTransportValueType(this string valueType)
        {
            if (valueType.ToLower().StartsWith("play") || valueType.ToLower().StartsWith("start"))
            {
                return TransportUpdateValueType.Play;
            }
            if (valueType.ToLower().StartsWith("stop"))
            {
                return TransportUpdateValueType.Stop;
            }
            if (valueType.ToLower().StartsWith("record"))
            {
                return TransportUpdateValueType.Record;
            }
            if (valueType.ToLower().StartsWith("cycle"))
            {
                return TransportUpdateValueType.Cycle;
            }
            if (valueType.ToLower().StartsWith("punchin"))
            {
                return TransportUpdateValueType.PunchIn;
            }
            if (valueType.ToLower().StartsWith("punchout"))
            {
                return TransportUpdateValueType.PunchOut;
            }
            return TransportUpdateValueType.Play;
        }

        public static int ToBooleanInt(this string boolVal)
        {
            if (boolVal.ToLower().StartsWith("on"))
            {
                return 1;
            }
            if (boolVal.ToLower().StartsWith("yes"))
            {
                return 1;
            }
            if (boolVal.ToLower().StartsWith("true"))
            {
                return 1;
            }
            if (boolVal.ToLower().StartsWith("off"))
            {
                return 0;
            }
            if (boolVal.ToLower().StartsWith("no"))
            {
                return 0;
            }
            if (boolVal.ToLower().StartsWith("false"))
            {
                return 0;
            }
            return 0;

        }

        public static int BoolToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        public static bool IntToBool(this int value)
        {
            return value == 1 ? true : false;
        }

        public static int GetTrackValueFromTrackValueType(this Track track, TrackUpdateValueType trackUpdateValueType)
        {
            switch (trackUpdateValueType)
            {
                case TrackUpdateValueType.Record:
                    return track.RecordEnable.BoolToInt();
                case TrackUpdateValueType.Mute:
                    return track.Mute.BoolToInt();
                case TrackUpdateValueType.Solo:
                    return track.Solo.BoolToInt();
                case TrackUpdateValueType.Selected:
                    return track.Selected.BoolToInt();
                default:
                    return 0;
            }
        }

        public static int GetTransportValueFromTransportValueType(this TransportModel model, TransportUpdateValueType transportUpdateValueType)
        {
            switch (transportUpdateValueType)
            {
                case TransportUpdateValueType.Play:
                    return model.Play.BoolToInt();
                case TransportUpdateValueType.Stop:
                    return model.Stop.BoolToInt();
                case TransportUpdateValueType.Record:
                    return model.Record.BoolToInt();
                case TransportUpdateValueType.Cycle:
                    return model.Cycle.BoolToInt();
                case TransportUpdateValueType.PunchIn:
                    return model.PunchIn.BoolToInt();
                case TransportUpdateValueType.PunchOut:
                    return model.PunchOut.BoolToInt();
                default:
                    return 0;
            }
        }
    }
}
