namespace Cubase.Macro.Services.Keyboard
{
    public interface IKeyboardService
    {
        bool SendKey(string key, Action<string> errHandler);
    }
}
