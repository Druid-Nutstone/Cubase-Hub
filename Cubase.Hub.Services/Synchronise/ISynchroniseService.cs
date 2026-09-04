namespace Cubase.Hub.Services.Synchronise
{
    public interface ISynchroniseService
    {
        void RegisterForEvent(Action<SyncEvent> eventHandler);

        void RaiseEvent(SyncEvent syncEvent);

    }
}
