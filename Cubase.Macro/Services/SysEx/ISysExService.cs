namespace Cubase.Macro.Services.SysEx
{
    public interface ISysExService
    {
        IEnumerable<string> SysExCommands { get; }

        bool ExecuteSysExCommand(string commandName, Action<string> onError);
    }
}
