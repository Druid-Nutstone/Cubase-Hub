using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Messages
{
    public class MessageService : IMessageService
    {                                           
        private List<Action<string, bool>> MessageProviders = new List<Action<string, bool>>();
        
        public void RegisterForMessages(Action<string, bool> provider)
        {
            this.MessageProviders.Add(provider);    
        }

        public void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);   
        }

        public void ShowMessage(string message, bool waitCursor)
        {
            foreach (var provider in this.MessageProviders)
            {
                provider.Invoke(message, waitCursor);
            }
        }
    }
}
