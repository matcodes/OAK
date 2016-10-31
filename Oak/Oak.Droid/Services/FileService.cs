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
using Oak.Services;
using Xamarin.Forms;
using Oak.Droid.Services;

[assembly: Dependency(typeof(FileService))]
namespace Oak.Droid.Services
{
    #region FileService
    public class FileService : IFileService
    {
        public FileService() : base()
        {
            var appWorkPath = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/OAK/");
            if (!appWorkPath.Exists())
                appWorkPath.Mkdirs();

            this.AppWorkPath = appWorkPath.ToString();
        }

        #region IFileService
        public string AppWorkPath { get; private set; }
        #endregion
    }
    #endregion
}