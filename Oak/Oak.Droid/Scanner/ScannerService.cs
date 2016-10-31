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
        public static readonly string TAG = "ScannerService";

        public static readonly UUID DEVICE_UUID = UUID.FromString("0000110a-0000-1000-8000-00805f9b34fb");
        public static readonly string DEVICE_NAME = "Oak FS-1";
        #endregion

        private readonly BluetoothAdapter _adapter = null;
        private BluetoothDevice _device = null;
        private BluetoothGatt _bluetoothGatt = null;

        private readonly List<BluetoothDevice> _devices = new List<BluetoothDevice>();

        private readonly Receiver _receiver = null;
        private readonly ScannerServiceCallback _scannerServiceCallback = null;

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

            _scannerServiceCallback = new ScannerServiceCallback(this);
        }

        #region IScannerService
        public void FindDevice()
        {
            if (_adapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!_adapter.IsEnabled)
            {
                var result = _adapter.Enable();
                if (!result)
                    throw new Exception("Bluetooth adapter is not enabled.");
            }

            if (_adapter.IsDiscovering)
                _adapter.CancelDiscovery();

            _adapter.StartDiscovery();
        }

        public async Task<bool> ConnectAsync()
        {
            var startTime = DateTime.Now;

            var isWait = true;

            while (isWait)
            {
                if (_receiver.Device != null)
                    isWait = false;
                else {
                    var timeout = DateTime.Now - startTime;
                    if (timeout.TotalMilliseconds > this.Timeout)
                        isWait = false;
                }
                Task.Delay(50).Wait();
            }

            if (_receiver.Device == null)
                throw new Exception("Scanner not found.");

            _device = _receiver.Device;

            _bluetoothGatt = _device.ConnectGatt(Xamarin.Forms.Forms.Context, false, _scannerServiceCallback);

            return true;
        }

        public int Timeout { get; set; } = 10000;
        #endregion

        #region Receiver
        public class Receiver : BroadcastReceiver
        {
            private BluetoothDevice _device = null;

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
                    if (device.Name.Equals(ScannerService.DEVICE_NAME))
                        _device = device;
                }
                else if (action == BluetoothAdapter.ActionDiscoveryFinished)
                {
                }
            }
            #endregion

            public BluetoothDevice Device { get; private set; }
        }
        #endregion

        #region ScannerServiceCallback
        public class ScannerServiceCallback : BluetoothGattCallback
        {
            private ScannerService _scannerService;

            public ScannerServiceCallback(ScannerService scannerService)
            {
                _scannerService = scannerService;
            }

            public override void OnConnectionStateChange(BluetoothGatt gatt, GattStatus status, ProfileState newState)
            {
                if (newState == ProfileState.Connected)
                {
                    Android.Util.Log.Info(ScannerService.TAG, "Connected to GATT server.");

                }
                else if (newState == ProfileState.Disconnected)
                {
                    Android.Util.Log.Info(ScannerService.TAG, "Disconnected from GATT server.");
                }
            }

            public override void OnServicesDiscovered(BluetoothGatt gatt, GattStatus status)
            {
                Android.Util.Log.Warn(ScannerService.TAG, "OnServicesDiscovered received: " + status);
            }

            public override void OnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
            {
                Android.Util.Log.Warn(ScannerService.TAG, "OnCharacteristicRead received: " + status);
            }

            public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
            {
            }
        }
        #endregion
    }
    #endregion
}