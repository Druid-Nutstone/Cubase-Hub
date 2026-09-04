namespace Cubase.Hub.Services.Synchronise
{
    public class SynchroniseService : ISynchroniseService
    {
        private List<Action<SyncEvent>> eventHandlers = new List<Action<SyncEvent>>();

        public void RaiseEvent(SyncEvent syncEvent)
        {
            foreach (var handler in eventHandlers)
            {
                handler.Invoke(syncEvent);
            }
        }

        public void RegisterForEvent(Action<SyncEvent> eventHandler)
        {
            if (!this.eventHandlers.Contains(eventHandler))
            {
                this.eventHandlers.Add(eventHandler);
            }
        }
    }
}
