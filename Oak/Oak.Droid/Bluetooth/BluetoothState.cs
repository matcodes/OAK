using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Oak.Droid.Bluetooth
{
    #region BluetoothState
    public class BluetoothState
    {
        public static readonly int STATE_NONE = 0;
        public static readonly int STATE_LISTEN = 1;
        public static readonly int STATE_CONNECTING = 2;
        public static readonly int STATE_CONNECTED = 3;
        public static readonly int STATE_NULL = -1;

        public static readonly int MESSAGE_STATE_CHANGE = 1;
        public static readonly int MESSAGE_READ = 2;
        public static readonly int MESSAGE_WRITE = 3;
        public static readonly int MESSAGE_DEVICE_NAME = 4;
        public static readonly int MESSAGE_TOAST = 5;

        public static readonly int REQUEST_CONNECT_DEVICE = 384;
        public static readonly int REQUEST_ENABLE_BT = 385;

        public static readonly string DEVICE_NAME = "device_name";
        public static readonly string DEVICE_ADDRESS = "device_address";
        public static readonly string TOAST = "toast";

        public static readonly bool DEVICE_ANDROID = true;
        public static readonly bool DEVICE_OTHER = false;

        public static readonly string EXTRA_DEVICE_ADDRESS = "device_address";
    }
    #endregion
}