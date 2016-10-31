using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Classes.Messages
{
    #region ShowToastMessage
    public class ShowToastMessage
    {
        #region Static members
        private static readonly string MESSAGE_KEY = "ShowToastMessage";

        public static void Send(string text)
        {
            var message = new ShowToastMessage(text);
            MessagingCenter.Send<ShowToastMessage>(message, MESSAGE_KEY);
        }

        public static void Subscribe(object subscriber, Action<ShowToastMessage> action)
        {
            MessagingCenter.Subscribe<ShowToastMessage>(subscriber, MESSAGE_KEY, action);
        }

        public static void Unsubscribe(object subscriber)
        {
            MessagingCenter.Unsubscribe<ShowToastMessage>(subscriber, MESSAGE_KEY);
        }
        #endregion

        public ShowToastMessage(string text)
        {
            this.Text = text;
        }

        public string Text { get; private set; }
    }
    #endregion
}
