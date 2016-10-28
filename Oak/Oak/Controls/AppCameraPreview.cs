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
        #endregion

        public AppCameraPreview() : base()
        {
        }

        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }
    }
    #endregion
}
