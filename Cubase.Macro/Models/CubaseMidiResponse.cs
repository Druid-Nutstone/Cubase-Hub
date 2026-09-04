namespace Cubase.Macro.Models
{
    public class CubaseMidiResponse
    {
        public string Name { get; set; }

        public int Channel { get; set; } = -1;

        public int Note { get; set; } = -1;
    }
}
