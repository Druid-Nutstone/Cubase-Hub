using Cubase.Hub.Forms.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Messages
{
    public class MessageService : IMessageService
    {                                           
        private List<Action<string, bool>> MessageProviders = new List<Action<string, bool>>();

        public NonBlockingMessage OpenMessage(string message, Control parent)
        {
            var messageForm = new NonBlockingMessage();
            messageForm.SetMessage(message);
            messageForm.StartPosition = FormStartPosition.Manual;
            messageForm.Location = new Point(
                parent.Left + (parent.Width - messageForm.Width) / 2,
                parent.Top + (parent.Height - messageForm.Height) / 2
            );

            // Remove any layering / Mica inheritance
            messageForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            messageForm.ShowInTaskbar = false;
            messageForm.TopMost = true;

            messageForm.Show();  // <-- no owner
            return messageForm;
        }

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
