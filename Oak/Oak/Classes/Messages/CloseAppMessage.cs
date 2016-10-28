using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Classes.Messages
{
    #region CloseAppMessage
    public class CloseAppMessage
    {
        #region Static members
        private static readonly string MESSAGE_KEY = "CloseAppMessage";

        public static void Send()
        {
            var message = new CloseAppMessage();
            MessagingCenter.Send<CloseAppMessage>(message, MESSAGE_KEY);
        }

        public static void Subscribe(object subscriber, Action<CloseAppMessage> action)
        {
            MessagingCenter.Subscribe<CloseAppMessage>(subscriber, MESSAGE_KEY, action);
        }

        public static void Unsubscribe(object subscriber)
        {
            MessagingCenter.Unsubscribe<CloseAppMessage>(subscriber, MESSAGE_KEY);
        }
        #endregion

        public CloseAppMessage()
        {
        }
    }
    #endregion
}
