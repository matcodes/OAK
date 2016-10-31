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
        public static readonly UUID DEVICE_UUID = UUID.FromString("ea9d5e37-dd5c-41d7-915c-624ec0151510"); //"00001101 -0000-1000-8000-00805f9b34fb");
        public static readonly string DEVICE_NAME = "Oak FS-1";
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

            while (_receiver.Device == null)
            {
                Task.Delay(50).Wait();
            }

            _adapter.CancelDiscovery();

            _device = _receiver.Device; //  _adapter.GetRemoteDevice(_receiver.Device.Address);

            var uuids = _device.GetUuids();
            if (uuids != null)
            {
                for (int j = 0; j < uuids.Length; j++)
                {
                    Console.WriteLine("{0} == {1}", uuids[j], DEVICE_UUID);
                    if (DEVICE_UUID.Equals(uuids[j]))
                    {
                        Console.WriteLine(uuids[j]);
                    }
                }
            }

            try
            {
                _socket = _device.CreateRfcommSocketToServiceRecord(DEVICE_UUID);
                //_socket = _device.CreateInsecureRfcommSocketToServiceRecord(DEVICE_UUID);
                await _socket.ConnectAsync();
            }
            catch
            {
                try
                {
                    _socket.Close();
                }
                catch
                {
                    throw;
                }
                throw;
            }

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

                    if (device.Name == ScannerService.DEVICE_NAME)
                        this.Device = device;
                }
                else if (action == BluetoothAdapter.ActionDiscoveryFinished)
                {
                    if (this.Device != null)
                    {
                        var res = this.Device.FetchUuidsWithSdp();
                        Console.WriteLine("FetchUuidsWithSdp^ {0}", res);
                    }
                }
            }
            #endregion

            public BluetoothDevice Device { get; private set; }
        }
        #endregion
    }
    #endregion
}