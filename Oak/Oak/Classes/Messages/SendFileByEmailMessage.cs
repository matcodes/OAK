using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Classes.Messages
{
    #region SendFileByEmailMessage
    public class SendFileByEmailMessage
    {
        #region Static members
        private static readonly string MESSAGE_KEY = "SendFileByEmailMessage";

        public static void Send(string fileName)
        {
            var message = new SendFileByEmailMessage(fileName);
            MessagingCenter.Send<SendFileByEmailMessage>(message, MESSAGE_KEY);
        }

        public static void Subscribe(object subscriber, Action<SendFileByEmailMessage> action)
        {
            MessagingCenter.Subscribe<SendFileByEmailMessage>(subscriber, MESSAGE_KEY, action);
        }

        public static void Unsubscribe(object subscriber)
        {
            MessagingCenter.Unsubscribe<SendFileByEmailMessage>(subscriber, MESSAGE_KEY);
        }
        #endregion

        public SendFileByEmailMessage(string fileName)
        {
            this.FileName = fileName;
        }

        public string FileName { get; private set; }
    }
    #endregion
}
