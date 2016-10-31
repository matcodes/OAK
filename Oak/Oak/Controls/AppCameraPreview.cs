using Oak.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Controls
{
    #region AppCameraPreview
    public class AppCameraPreview : View
    {
        #region Static members
        public static readonly BindableProperty CameraProperty = BindableProperty.Create("Camera", typeof(CameraOptions), typeof(AppCameraPreview), CameraOptions.Rear);
        public static readonly BindableProperty TakePhotoProperty = BindableProperty.Create("TakePhoto", typeof(bool), typeof(AppCameraPreview), false);
        #endregion

        public AppCameraPreview() : base()
        {
        }

        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public bool TakePhoto
        {
            get { return (bool)this.GetValue(TakePhotoProperty); }
            set { this.SetValue(TakePhotoProperty, value); }
        }
    }
    #endregion
}
