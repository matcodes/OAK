using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Oak.Services;
using Android.Bluetooth;
using Java.Util;
using Oak.Droid.Scanner;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScannerService))]
namespace Oak.Droid.Scanner
{
    #region ScannerService
    public class ScannerService : IScannerService
    {
        #region Static members
        private static readonly UUID DEVICE_UUID = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
        private static readonly string DEVICE_NAME = "[TV]Samsung LED32";
        #endregion

        private readonly BluetoothAdapter _adapter = null;
        private BluetoothDevice _device = null;
        private BluetoothSocket _socket = null;

        private readonly List<BluetoothDevice> _devices = new List<BluetoothDevice>();

        private readonly Receiver _receiver = null;

        public ScannerService() : base()
        {
            _adapter = BluetoothAdapter.DefaultAdapter;

            _devices.Clear();

            // Register for broadcasts when a device is discovered
            _receiver = new Receiver();
            var filter = new IntentFilter();
            filter.AddAction(BluetoothDevice.ActionFound);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);
            Xamarin.Forms.Forms.Context.RegisterReceiver(_receiver, filter);

            if (_adapter.IsDiscovering)
                _adapter.CancelDiscovery();

            _adapter.StartDiscovery();
        }

        #region IScannerService
        public async Task<bool> ConnectAsync()
        {
            var bytes = new byte[] { 122, 123, 123, 118, 115 };
            var str = Encoding.UTF8.GetString(bytes);
            StringTokenizer st1 = new StringTokenizer(str, ",");
            var st2 = st1.NextToken();

            if (_adapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!_adapter.IsEnabled)
            {
                var result = _adapter.Enable();
                if (!result)
                    throw new Exception("Bluetooth adapter is not enabled.");
            }

            await Task.Run(() => { });

            _device = _adapter.BondedDevices.FirstOrDefault(d => d.Name == DEVICE_NAME);
            if (_device == null)
                throw new Exception("Named device not found.");

            _socket = _device.CreateRfcommSocketToServiceRecord(DEVICE_UUID);
            //_socket = _device.CreateInsecureRfcommSocketToServiceRecord(DEVICE_UUID);
            await _socket.ConnectAsync();

            return true;
        }
        #endregion

        #region Receiver
        public class Receiver : BroadcastReceiver
        {
            private List<BluetoothDevice> _devices = new List<BluetoothDevice>();

            public Receiver()
            {
            }

            #region OnReceiver
            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;

                if (action == BluetoothDevice.ActionFound)
                {
                    BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    _devices.Add(device);
                }
                else if (action == BluetoothAdapter.ActionDiscoveryFinished)
                {
                }
            }
            #endregion
        }
        #endregion
    }
    #endregion
}