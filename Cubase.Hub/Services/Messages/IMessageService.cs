using Cubase.Hub.Forms.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Messages
{
    public interface IMessageService
    {
        void RegisterForMessages(Action<string, bool> provider);

        void ShowMessage(string message, bool waitCursor);   
    
        void ShowError(string errorMessage); 
        
        NonBlockingMessage OpenMessage(string message, Control parent); 

        DialogResult AskMessage(string message);   

    }
}
