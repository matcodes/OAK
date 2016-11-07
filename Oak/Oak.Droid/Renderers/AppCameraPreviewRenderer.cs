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
using Oak.Controls;
using Oak.Droid.Controls;
using Xamarin.Forms.Platform.Android;
using Android.Hardware;
using Xamarin.Forms;
using Oak.Droid.Renderers;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(AppCameraPreview), typeof(AppCameraPreviewRenderer))]
namespace Oak.Droid.Renderers
{
    #region AppCameraPreviewRenderer
    public class AppCameraPreviewRenderer : ViewRenderer<AppCameraPreview, CameraPreview>, CameraPreview.ICameraPreviewCallback
    {
        private CameraPreview _cameraPreview;

        protected override void OnElementChanged(ElementChangedEventArgs<AppCameraPreview> args)
        {
            base.OnElementChanged(args);

            if (this.Control == null)
            {
                _cameraPreview = new CameraPreview(Context);
                _cameraPreview.CameraPreviewCallback = this;
                SetNativeControl(_cameraPreview);
            }

            if (args.OldElement != null)
            {
                _cameraPreview.Click -= OnCameraPreviewClicked;
            }
            if (args.NewElement != null)
            {
                Control.Preview = Camera.Open((int)args.NewElement.Camera);

                _cameraPreview.Click += OnCameraPreviewClicked;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            var cameraPreview = (this.Element as AppCameraPreview);

            if ((args.PropertyName == AppCameraPreview.TakePhotoProperty.PropertyName) && (cameraPreview.TakePhoto))
            {
                this.Control.TakePicture();
                cameraPreview.TakePhoto = false;
            }
        }

        private void OnCameraPreviewClicked(object sender, EventArgs args)
        {
            //if (_cameraPreview.IsPreviewing)
            //{
            //    _cameraPreview.Preview.StopPreview();
            //    _cameraPreview.IsPreviewing = false;
            //}
            //else
            //{
            //    _cameraPreview.Preview.StartPreview();
            //    _cameraPreview.IsPreviewing = true;
            //}
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.Preview.Release();
            }
            base.Dispose(disposing);
        }

        #region CameraPreview.ICameraPreviewCallback
        public void AfterPictureTaken(string fileName)
        {
            if (this.Element != null)
                this.Element.LastFileName = fileName;
        }
        #endregion
    }
    #endregion
}