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
using Oak.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScannerService))]
namespace Oak.Droid.Services
{
    #region ScannerService
    public class ScannerService : IScannerService
    {
        #region Static members
        public static readonly string TAG = "ScannerService";

        public static readonly UUID SERVICE_UUID = UUID.FromString("ea9d5e37-dd5c-41d7-915c-624ec0151510");
        public static readonly UUID COMMAND_UUID = UUID.FromString("ea9d5e37-dd5c-41d7-915c-624ec0151513");
        public static readonly UUID DATA_UUID = UUID.FromString("ea9d5e37-dd5c-41d7-915c-624ec0151512");
        public static readonly string DEVICE_NAME = "Oak1"; //"Oak FS-1";

        public static readonly int PACKAGE_COUNT = 1600;
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

        public void DiscoverServices()
        {
            _bluetoothGatt.DiscoverServices();
        }

        public void ReadData()
        {
            var bluetoothService = _bluetoothGatt.GetService(SERVICE_UUID);
            if (bluetoothService == null)
            {
                this.ReadDataErrorMessage = "Service of Scannner not found.";
                return;
            }

            var bluetoothData = bluetoothService.GetCharacteristic(DATA_UUID);
            if (bluetoothData == null)
            {
                this.ReadDataErrorMessage = "Data characteristic not found.";
                return;
            }

            if (!_bluetoothGatt.ReadCharacteristic(bluetoothData))
                this.ReadDataErrorMessage = "Failed to read Data.";
        }

        public void AddData(byte[] data)
        {
            var x = BitConverter.ToDouble(new byte[] { data[0], data[1], data[2], data[3] }, 0);
            var y = BitConverter.ToDouble(new byte[] { data[4], data[5], data[6], data[7] }, 0);
            var n = BitConverter.ToUInt16(new byte[] { data[8], data[9] }, 8);

            var scannerData = new ScannerData {
                X = BitConverter.ToUInt32(data, 0),
                Y = BitConverter.ToUInt32(data, 4),
                N = BitConverter.ToUInt16(data, 8),
            };
            this.Data.Add(scannerData);

            scannerData = new ScannerData {
                X = BitConverter.ToUInt32(data, 10),
                Y = BitConverter.ToUInt32(data, 14),
                N = BitConverter.ToUInt16(data, 18)
            };
            this.Data.Add(scannerData);

            this.PackageCount++;

            if (this.PackageCount < PACKAGE_COUNT)
                this.ReadData();
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

        public bool Connect()
        {
            this.IsConnected = false;

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

            isWait = true;
            while (isWait)
            {
                isWait = !this.IsConnected;
                if (isWait)
                {
                    var timeout = DateTime.Now - startTime;
                    if (timeout.TotalMilliseconds > this.Timeout)
                        throw new Exception("Scanner connection timeout.");
                    Task.Delay(50).Wait();
                }
            }

            return this.IsConnected;
        }

        public ScannerData[] Scan()
        {
            //var startTime = DateTime.Now;

            this.Data.Clear();
            this.PackageCount = 0;
            this.ReadDataErrorMessage = "";

            if (!this.IsConnected)
                throw new Exception("Scanner connection failed.");

            var bluetoothService = _bluetoothGatt.GetService(SERVICE_UUID);
            if (bluetoothService == null)
                throw new Exception("Service of Scannner not found.");

            var bluetoothCommand = bluetoothService.GetCharacteristic(COMMAND_UUID);
            if (bluetoothCommand == null)
                throw new Exception("Command characteristic not found.");

            bluetoothCommand.SetValue("pb");
            if (!_bluetoothGatt.WriteCharacteristic(bluetoothCommand))
                throw new Exception("Failed to write Command (Push Button).");

            var isWait = true;
            while (isWait)
            {
                if (this.PackageCount >= PACKAGE_COUNT)
                    isWait = false;

                else
                {
                    if (!String.IsNullOrEmpty(this.ReadDataErrorMessage))
                        throw new Exception(this.ReadDataErrorMessage);

                    //var timeout = (DateTime.Now - startTime);
                    //if (timeout.TotalMilliseconds > this.Timeout)
                    //    throw new Exception("Read data timeout.");

                    Task.Delay(50).Wait();
                }
            }

            return this.Data.ToArray();
        }

        public int Timeout { get; set; } = 30000;

        public int PackageCount { get; set; } = 0;

        public List<ScannerData> Data { get; set; } = new List<ScannerData>();

        public string ReadDataErrorMessage = "";

        public bool IsConnected { get; set; } = false;
        #endregion

        #region Receiver
        public class Receiver : BroadcastReceiver
        {
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
                    if ((!String.IsNullOrEmpty(device.Name)) && (device.Name.Equals(ScannerService.DEVICE_NAME)))
                        this.Device = device;
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
                    _scannerService.IsConnected = true;
                    _scannerService.DiscoverServices();

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
                if (status == GattStatus.Success)
                {
                    var data = characteristic.GetValue();
                    _scannerService.AddData(data);
                }
                else
                    _scannerService.ReadDataErrorMessage = "Failed to read Data.";

                Android.Util.Log.Warn(ScannerService.TAG, "OnCharacteristicRead received: " + status);
            }

            public override void OnCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, [GeneratedEnum] GattStatus status)
            {
                if (status == GattStatus.Success)
                    _scannerService.ReadData();
                else
                    _scannerService.ReadDataErrorMessage = "Failed to write Data.";

                base.OnCharacteristicWrite(gatt, characteristic, status);
            }

            public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
            {
            }
        }
        #endregion
    }
    #endregion
}