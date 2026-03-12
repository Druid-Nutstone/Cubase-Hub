using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Synchronise
{
    public interface ISynchroniseService
    {
        void RegisterForEvent(Action<SyncEvent> eventHandler);
    
        void RaiseEvent(SyncEvent syncEvent);   
    
    }
}
