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
using Android.Bluetooth;
using Java.Lang;
using Java.Util;
using System.Threading.Tasks;
using System.IO;

namespace Oak.Droid.Bluetooth
{
    #region BluetoothService
    public class BluetoothService
    {
        #region Static members
        private static readonly string TAG = "Bluetooth Service";

        private static readonly string NAME_SECURITY = "Bluetooth Security";

        private static readonly int DATA_BYTES = 64;

        private static readonly UUID UUID_ANDROID_DEVICE = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        private static readonly UUID UUID_OTHER_DEVICE = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        #endregion

        private readonly BluetoothAdapter _adapter = null;
        private readonly Handler _handler = null;
        private int _state = BluetoothState.STATE_NONE;
        private bool _isAndroid = BluetoothState.DEVICE_ANDROID;

        private readonly object _stateLocker = new object();
        private readonly object _acceptLocket = new object();
        private readonly object _connectLocker = new object();

        private BluetoothSocket _socket = null;
        private Stream _inputStream = null;
        private Stream _outputStream = null;

        public BluetoothService(Context context, Handler handler)
        {
            _adapter = BluetoothAdapter.DefaultAdapter;
            _handler = handler;
        }

        public void Start(bool isAndroid)
        {
            this.CancelConnect();
            this.CancelConnected();

            this.State = BluetoothState.STATE_LISTEN;

            this.RunAccept(isAndroid);

            _isAndroid = isAndroid;
        }

        public void Stop()
        {
            this.CancelConnect();
            this.CancelConnected();
            this.CancelAccept();

            this.State = BluetoothState.STATE_NONE;
        }

        public void Connect()
        {
            lock (_connectLocker)
            {
                if (this.State == BluetoothState.STATE_CONNECTING)
                    this.CancelConnect();

                this.CancelConnected();

                this.RunConnect();

                this.State = BluetoothState.STATE_CONNECTING;
            }
        }

        public void Connected()
        {
            this.CancelConnect();
            this.CancelConnected();

            this.CancelAccept();

            this.RunConnected(_socket);

            Message message = _handler.ObtainMessage(BluetoothState.MESSAGE_DEVICE_NAME);
            var bundle = new Bundle();
            bundle.PutString(BluetoothState.DEVICE_NAME, _socket.RemoteDevice.Name);
            bundle.PutString(BluetoothState.DEVICE_ADDRESS, _socket.RemoteDevice.Address);
            message.Data = bundle;
            _handler.SendMessage(message);

            this.State = BluetoothState.STATE_CONNECTED;
        }

        private void SetState(int state)
        {
            lock (_stateLocker)
            {
                System.Console.WriteLine("{0}: SetState() -> {1}", TAG, state);
                _state = state;
                _handler.ObtainMessage(BluetoothState.MESSAGE_STATE_CHANGE, state, -1).SendToTarget();
            }
        }

        private int GetState()
        {
            lock (_stateLocker)
            {
                return _state;
            }
        }

        private void ConnectionLost()
        {
            this.Start(_isAndroid);
        }

        private void ConnectionFailed()
        {
            this.Start(_isAndroid);
        }

        public int State
        {
            get { return this.GetState(); }
            set { this.SetState(value); }
        }

        #region AcceptThread
        private bool _isAcceptRunning = false;
        private BluetoothServerSocket _acceptServerSocket = null;

        private void RunAccept(bool isAndroid)
        {
            Task.Run(() => {
                try
                {
                    var uuid = (isAndroid ? UUID_ANDROID_DEVICE : UUID_OTHER_DEVICE);
                    _acceptServerSocket = _adapter.ListenUsingRfcommWithServiceRecord(NAME_SECURITY, uuid);
                }
                catch (IOException)
                {
                }

                _isAcceptRunning = true;

                BluetoothSocket socket = null;

                while ((this.State != BluetoothState.STATE_CONNECTED) && (_isAcceptRunning))
                {
                    try
                    {
                        socket = _acceptServerSocket.Accept();
                    }
                    catch (IOException)
                    {
                        break;
                    }

                    if (socket != null)
                        lock (_acceptLocket)
                        {
                            if ((this.State == BluetoothState.STATE_LISTEN) || (this.State == BluetoothState.STATE_CONNECTING))
                            {
 //                               this.Connected(socket, socket.RemoteDevice);
                            }
                            else if ((this.State == BluetoothState.STATE_CONNECTED) || (this.State == BluetoothState.STATE_NONE))
                            {
                                try
                                {
                                    socket.Close();
                                }
                                catch (IOException)
                                {
                                }
                            }
                        }
                }

            });
        }

        private void CancelAccept()
        {
            try
            {
                _isAcceptRunning = false;
                if (_acceptServerSocket != null)
                {
                    _acceptServerSocket.Close();
                    _acceptServerSocket = null;
                }
            }
            catch (IOException)
            {
            }
        }
        #endregion

        #region ConnectThread
        private void RunConnect()
        {
            Task.Run(() => {
                BluetoothSocket socket = null;
                try
                {
                    var uuid = (_isAndroid ? UUID_ANDROID_DEVICE : UUID_OTHER_DEVICE);
                    socket = _socket.RemoteDevice.CreateRfcommSocketToServiceRecord(uuid);
                }
                catch
                {
                }

                _adapter.CancelDiscovery();

                try
                {
                    socket.Connect();
                }
                catch
                {
                    try
                    {
                        socket.Close();
                    }
                    catch
                    {
                        this.ConnectionFailed();
                    }
                }

                _socket = socket;
                this.Connected();
            });
        }

        private void CancelConnect()
        {
        }
        #endregion

        #region ConnectedThread
        private void RunConnected(BluetoothSocket socket)
        {
            try
            {
                _inputStream = socket.InputStream;
                _outputStream = socket.OutputStream;
            }
            catch (IOException)
            {
            }

            Task.Run(() => {
                while (true)
                {
                    try
                    {
                        var buffer = new byte[DATA_BYTES];
                        _inputStream.Read(buffer, 0, DATA_BYTES);

                        _handler.ObtainMessage(BluetoothState.MESSAGE_READ, buffer.Length, -1, buffer);
                    }
                    catch 
                    {
                        this.ConnectionLost();
                        break;
                    }
                }
            });
        }

        private void CancelConnected()
        {
        }
       #endregion
    }
    #endregion
}