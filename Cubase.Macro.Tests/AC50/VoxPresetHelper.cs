using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VoxPresetTools
{
    public class VoxPreset
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "New Preset";

        [JsonPropertyName("parameters")]
        public PresetParameters Parameters { get; set; } = new PresetParameters();
    }

    public class AC50Preset
    {
        [JsonPropertyName("amp_model")]
        public string AmpModel { get; set; }

        [JsonPropertyName("gain")]
        public double Gain { get; set; }

        [JsonPropertyName("treble")]
        public double Treble { get; set; }

        [JsonPropertyName("middle")]
        public double Middle { get; set; }

        [JsonPropertyName("bass")]
        public double Bass { get; set; }

        [JsonPropertyName("noise_reduction")]
        public double NoiseReduction { get; set; }

        [JsonPropertyName("volume")]
        public double Volume { get; set; }

        [JsonPropertyName("modulation_type")]
        public string ModulationType { get; set; }

        [JsonPropertyName("modulation_speed")]
        public double ModulationSpeed { get; set; }

        [JsonPropertyName("delay_reverb_mix")]
        public double DelayReverbMix { get; set; }

        [JsonPropertyName("delay_time_ms")]
        public double DelayTimeMs { get; set; }

        [JsonPropertyName("reverb_type")]
        public string ReverbType { get; set; }
    }

    public class PresetParameters
    {
        [JsonPropertyName("block_marker")]
        public byte BlockMarker { get; set; }

        [JsonPropertyName("amp_model")]
        public byte AmpModel { get; set; }

        [JsonPropertyName("gain")]
        public byte Gain { get; set; }

        [JsonPropertyName("treble")]
        public byte Treble { get; set; }

        [JsonPropertyName("middle")]
        public byte Middle { get; set; }

        [JsonPropertyName("bass")]
        public byte Bass { get; set; }

        [JsonPropertyName("noise_reduction")]
        public byte NoiseReduction { get; set; }

        [JsonPropertyName("volume")]
        public byte Volume { get; set; }

        [JsonPropertyName("modulation_type")]
        public byte ModulationType { get; set; }

        [JsonPropertyName("modulation_speed")]
        public byte ModulationSpeed { get; set; }

        [JsonPropertyName("delay_reverb_mix")]
        public byte DelayReverbMix { get; set; }

        [JsonPropertyName("delay_time_ms")]
        public ushort DelayTimeMs { get; set; }

        [JsonPropertyName("reverb_type")]
        public byte ReverbType { get; set; }
    }

    public class VoxPresetManager
    {
        private static readonly byte[] MagicHeader = {
            0x43, 0x41, 0x4D, 0x42, 0x52, 0x47, 0x35, 0x30, 0x50, 0x52, 0x4F, 0x47, 0x00, 0x00, 0x00, 0x00
        };

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        /// <summary>
        /// Reads all preset programs contained within a bank file (.cam50prog) safely.
        /// </summary>
        public List<VoxPreset> ReadBankFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var presets = new List<VoxPreset>();

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(fs, Encoding.ASCII);

            if (fs.Length < 32)
                throw new InvalidDataException("File is too small to be a valid Vox bank file.");

            // 1. Validate 16-Byte File Header
            byte[] header = reader.ReadBytes(16);
            for (int i = 0; i < 12; i++)
            {
                if (header[i] != MagicHeader[i])
                    throw new InvalidDataException("Invalid file header: Not a Vox Cambridge50 file.");
            }

            // Skip global header padding (16 bytes)
            reader.ReadBytes(16);

            // 2. Loop through preset blocks safely
            while (fs.Position + 30 <= fs.Length)
            {
                // Read 16-byte preset name
                byte[] nameBytes = reader.ReadBytes(16);
                string name = Encoding.ASCII.GetString(nameBytes).Trim('\0', ' ');

                byte blockMarker = reader.ReadByte();
                byte ampModel = reader.ReadByte();
                byte unknownPadding = reader.ReadByte(); // 0x09 padding/flag

                var parameters = new PresetParameters
                {
                    BlockMarker = blockMarker,
                    AmpModel = ampModel,
                    Gain = reader.ReadByte(),  // Catches 0x0A (1.0)
                    Treble = reader.ReadByte(),  // Catches 0x14 (2.0)
                    Middle = reader.ReadByte(),  // Catches 0x1E (3.0)
                    Bass = reader.ReadByte(),  // Catches 0x28 (4.0)
                    NoiseReduction = reader.ReadByte(),
                    Volume = reader.ReadByte(),
                    ModulationType = reader.ReadByte(),
                    ModulationSpeed = reader.ReadByte(),
                    DelayReverbMix = reader.ReadByte(),
                    DelayTimeMs = reader.ReadUInt16(),
                    ReverbType = reader.ReadByte()
                };

                presets.Add(new VoxPreset
                {
                    Name = string.IsNullOrEmpty(name) ? "(Unnamed Preset)" : name,
                    Parameters = parameters
                });

                // Safely advance past any trailing block padding / footer bytes
                int bytesRemainingInBlockSlot = 6;
                for (int i = 0; i < bytesRemainingInBlockSlot && fs.Position < fs.Length; i++)
                {
                    reader.ReadByte();
                }
            }

            return presets;
        }

        /// <summary>
        /// Writes a collection of AC50Preset objects into a valid .cam50prog binary bank file.
        /// </summary>
        public void WriteBankFile(string filePath, IEnumerable<AC50Preset> presets, string bankName = "New Program 1")
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new BinaryWriter(fs, Encoding.ASCII);

            // 1. Write 16-Byte File Magic Header
            writer.Write(MagicHeader);

            // 2. Write 16 bytes of global header padding
            writer.Write(new byte[16]);

            int presetIndex = 0;
            foreach (var preset in presets)
            {
                presetIndex++;
                string nameToUse = presetIndex == 1 ? bankName : $"New Program {presetIndex}";

                byte[] nameBytes = new byte[16];
                Array.Clear(nameBytes, 0, nameBytes.Length);
                Encoding.ASCII.GetBytes(nameToUse).CopyTo(nameBytes, 0);
                writer.Write(nameBytes);

                // Parameter block bytes matching the verified schema
                writer.Write((byte)0x32); // Block Marker

                byte ampModelId = byte.TryParse(preset.AmpModel, out var parsedAmp) ? parsedAmp : (byte)60;
                writer.Write(ampModelId);
                writer.Write((byte)0x09); // Intermediate padding/flag byte

                writer.Write((byte)(preset.Gain * 10));
                writer.Write((byte)(preset.Treble * 10));
                writer.Write((byte)(preset.Middle * 10));
                writer.Write((byte)(preset.Bass * 10));
                writer.Write((byte)(preset.NoiseReduction * 10));
                writer.Write((byte)(preset.Volume * 10));

                byte modType = byte.TryParse(preset.ModulationType, out var parsedMod) ? parsedMod : (byte)4;
                writer.Write(modType);
                writer.Write((byte)preset.ModulationSpeed);

                writer.Write((byte)(preset.DelayReverbMix * 10));
                writer.Write((ushort)preset.DelayTimeMs);

                byte revType = byte.TryParse(preset.ReverbType, out var parsedRev) ? parsedRev : (byte)32;
                writer.Write(revType);

                // Write 6 trailing block padding bytes
                writer.Write(new byte[6]);
            }
        }

        public string ToJson(VoxPreset preset)
        {
            return JsonSerializer.Serialize(preset, _jsonOptions);
        }

        public void ExampleJson(string fileName)
        {
            var presetJson = new AC50Preset();
            var asText = JsonSerializer.Serialize(presetJson, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(fileName, asText);
        }

        public VoxPreset FromJson(string jsonString)
        {
            return JsonSerializer.Deserialize<VoxPreset>(jsonString, _jsonOptions)
                   ?? throw new InvalidDataException("Failed to deserialize JSON string.");
        }

        public void PrintPresetDetails(VoxPreset preset, string sourceName = "Preset", TextWriter output = null)
        {
            output ??= Console.Out;
            var p = preset.Parameters;

            output.WriteLine($"Preset Source  : {sourceName}");
            output.WriteLine($"----------------------------------------");
            output.WriteLine($"Preset Name    : {preset.Name}");
            output.WriteLine($"Block Marker   : 0x{p.BlockMarker:X2}");
            output.WriteLine($"Amp Model ID   : {p.AmpModel}");
            output.WriteLine($"Gain           : {p.Gain / 10.0f:F1}");
            output.WriteLine($"Treble         : {p.Treble / 10.0f:F1}");
            output.WriteLine($"Middle         : {p.Middle / 10.0f:F1}");
            output.WriteLine($"Bass           : {p.Bass / 10.0f:F1}");
            output.WriteLine($"Noise Red. (NR): {p.NoiseReduction / 10.0f:F1}");
            output.WriteLine($"Volume         : {p.Volume / 10.0f:F1}");
            output.WriteLine($"Modulation Type: {p.ModulationType}");
            output.WriteLine($"Mod Speed      : {p.ModulationSpeed}");
            output.WriteLine($"Delay/Rev Mix  : {p.DelayReverbMix / 10.0f:F1}");
            output.WriteLine($"Delay Time (ms): {p.DelayTimeMs}");
            output.WriteLine($"Reverb Type    : {p.ReverbType}");
            output.WriteLine($"----------------------------------------\n");
        }

        public void InspectFileHex(string filePath, TextWriter output = null)
        {
            output ??= Console.Out;

            if (!File.Exists(filePath))
            {
                output.WriteLine($"File not found for inspection: {filePath}");
                return;
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            output.WriteLine($"--- HEX INSPECTION: {filePath} ({fileBytes.Length} bytes) ---");
            output.WriteLine("Offset(h)  00 01 02 03 04 05 06 07  08 09 0A 0B 0C 0D 0E 0F   Decoded ASCII");
            output.WriteLine("-------------------------------------------------------------------------");

            for (int i = 0; i < fileBytes.Length; i += 16)
            {
                output.Write($"{i:X4}:      ");
                for (int j = 0; j < 16; j++)
                {
                    if (i + j < fileBytes.Length)
                        output.Write($"{fileBytes[i + j]:X2} ");
                    else
                        output.Write("   ");
                }
                output.Write("  ");
                for (int j = 0; j < 16 && i + j < fileBytes.Length; j++)
                {
                    char c = (char)fileBytes[i + j];
                    output.Write(char.IsControl(c) ? '.' : c);
                }
                output.WriteLine();
            }
            output.WriteLine("-------------------------------------------------------------------------\n");
        }
    }
}