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
using Android.Hardware;
using static Android.Hardware.Camera;
using Xamarin.Forms;
using Oak.Services;
using System.IO;
using System.Threading.Tasks;
using Android.Media;

namespace Oak.Droid.Controls
{
    #region CameraPreview
    public sealed class CameraPreview : ViewGroup, ISurfaceHolderCallback, IPictureCallback, IAutoFocusCallback
    {
        private SurfaceView _surfaceView;
        private ISurfaceHolder _holder;
        private Camera.Size _previewSize;
        private IList<Camera.Size> _supportedPreviewSizes;
        private Camera _camera;
        private IWindowManager _windowManager;

        public bool IsPreviewing { get; set; }

        public Camera Preview
        {
            get { return _camera; }
            set
            {
                _camera = value;
                if (_camera != null)
                {
                    _supportedPreviewSizes = Preview.GetParameters().SupportedPreviewSizes;
                    RequestLayout();
                }
            }
        }

        public CameraPreview(Context context)
            : base(context)
        {
            _surfaceView = new SurfaceView(context);
            _surfaceView.Touch += (sender, args) =>
            {
                var x = args.Event.GetX();
                var y = args.Event.GetY();

                var touchRect = new Android.Graphics.Rect(
                        (int)(x - 100),
                        (int)(y - 100),
                        (int)(x + 100),
                        (int)(y + 100));


                var targetFocusRect = new Android.Graphics.Rect(
                        touchRect.Left * 2000 / this.Width - 1000,
                        touchRect.Top * 2000 / this.Height - 1000,
                        touchRect.Right * 2000 / this.Width - 1000,
                        touchRect.Bottom * 2000 / this.Height - 1000);

                this.DoTouchFocus(targetFocusRect);
            };

            AddView(_surfaceView);

            _windowManager = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            IsPreviewing = false;
            _holder = _surfaceView.Holder;
            _holder.AddCallback(this);
        }

        public void DoTouchFocus(Android.Graphics.Rect focusRect)
        {
            try
            {
                var list = new List<Camera.Area>();
                Camera.Area focusArea = new Camera.Area(focusRect, 1000);
                list.Add(focusArea);

                var parameters = _camera.GetParameters();
                parameters.FocusAreas = list;
                parameters.MeteringAreas = list;
                _camera.SetParameters(parameters);

                _camera.AutoFocus(this);
            }
            catch (Exception exception)
            {
//                e.printStackTrace();
            }

            //if (isNeedToTakePic())
            //{
            //    onFocusListener.onFocused();
            //}
        }

        public void TakePicture()
        {
            if (this.Preview != null)
            {
                this.Preview.TakePicture(null, null, this);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = ResolveSize(this.SuggestedMinimumWidth, widthMeasureSpec);
            int height = ResolveSize(this.SuggestedMinimumHeight, heightMeasureSpec);
            SetMeasuredDimension(width, height);

            if (_supportedPreviewSizes != null)
            {
                _previewSize = GetOptimalPreviewSize(_supportedPreviewSizes, width, height);
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            _surfaceView.Measure(msw, msh);
            _surfaceView.Layout(0, 0, r - l, b - t);
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                if (this.Preview != null)
                    this.Preview.SetPreviewDisplay(holder);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            if (this.Preview != null)
                this.Preview.StopPreview();
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            var parameters = Preview.GetParameters();
            parameters.SetPreviewSize(_previewSize.Width, _previewSize.Height);
            this.RequestLayout();

            switch (_windowManager.DefaultDisplay.Rotation)
            {
                case SurfaceOrientation.Rotation0:
                    _camera.SetDisplayOrientation(90);
                    break;
                case SurfaceOrientation.Rotation90:
                    _camera.SetDisplayOrientation(0);
                    break;
                case SurfaceOrientation.Rotation270:
                    _camera.SetDisplayOrientation(180);
                    break;
            }

            this.Preview.SetParameters(parameters);
            this.Preview.StartPreview();
            IsPreviewing = true;
        }

        Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h)
        {
            const double _aspectTolerance = 0.1;
            double _targetRatio = (double)w / h;

            if (sizes == null)
            {
                return null;
            }

            Camera.Size optimalSize = null;
            double minDiff = double.MaxValue;

            int targetHeight = h;
            foreach (Camera.Size size in sizes)
            {
                double ratio = (double)size.Width / size.Height;

                if (Math.Abs(ratio - _targetRatio) > _aspectTolerance)
                    continue;
                if (Math.Abs(size.Height - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            if (optimalSize == null)
            {
                minDiff = double.MaxValue;
                foreach (Camera.Size size in sizes)
                {
                    if (Math.Abs(size.Height - targetHeight) < minDiff)
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }

            return optimalSize;
        }

        #region OnPictureTaken
        public void OnPictureTaken(byte[] data, Camera camera)
        {
            camera.StartPreview();
            Task.Run(() => {
                try
                {
                    var fileService = DependencyService.Get<IFileService>();

                    var fileName = "photo_" + DateTime.Now.ToString() + ".jpg";
                    fileName = fileName.Replace("/", "_");

                    fileName = System.IO.Path.Combine(fileService.AppWorkPath, fileName);

 //                   File.WriteAllBytes(fileName, data);

                    var rotate = 90;
                    if (rotate > 0)
                    {
                        var matrix = new Android.Graphics.Matrix();
                        matrix.PostRotate(rotate);

                        var bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length); // MediaStore.Images.Media.GetBitmap(this.ContentResolver, Android.Net.Uri.Parse(uri));
                        var rotatedBitmap = Android.Graphics.Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
                        bitmap.Recycle();
                        bitmap = rotatedBitmap;

                        using (var finalStream = new MemoryStream())
                        {
                            bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, finalStream);
                            data = finalStream.GetBuffer();
                        }

                        File.WriteAllBytes(fileName, data);

                        bitmap.Recycle();
                        GC.Collect();
                    }

                    if (this.CameraPreviewCallback != null)
                        this.CameraPreviewCallback.AfterPictureTaken(fileName);
                }
                catch
                {

                }
            });
        }

        private int GetRotation(string fileName)
        {
            var rotate = 0;
            if (!String.IsNullOrEmpty(fileName))
            {
                ExifInterface exif = new ExifInterface(fileName);
                //int orientation = exif.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_NORMAL);
                var orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Android.Media.Orientation.Normal);
                switch (orientation)
                {
                    case (int)Android.Media.Orientation.Rotate270:
                        rotate = 270;
                        break;
                    case (int)Android.Media.Orientation.Rotate180:
                        rotate = 180;
                        break;
                    case (int)Android.Media.Orientation.Rotate90:
                        rotate = 90;
                        break;
                }
            }
            return rotate;
        }
        #endregion

        #region IAutoFocusCallback
        public void OnAutoFocus(bool success, Camera camera)
        {
        }
        #endregion

        public ICameraPreviewCallback CameraPreviewCallback { get; set; }

        #region ICameraPreviewCallback
        public interface ICameraPreviewCallback
        {
            void AfterPictureTaken(string fileName);
        }
        #endregion
    }
    #endregion
}