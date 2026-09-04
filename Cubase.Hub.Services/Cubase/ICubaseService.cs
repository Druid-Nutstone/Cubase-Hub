namespace Cubase.Hub.Services.Cubase
{
    public interface ICubaseService
    {
        void OpenCubaseProject(string projectPath, Action<string>? OnError);
    }
}
