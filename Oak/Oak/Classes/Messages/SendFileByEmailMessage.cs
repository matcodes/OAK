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

        public static void Send(string[] fileNames)
        {
            var message = new SendFileByEmailMessage(fileNames);
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

        public SendFileByEmailMessage(string[] fileNames)
        {
            this.FileNames = fileNames;
        }

        public string[] FileNames { get; private set; }
    }
    #endregion
}
