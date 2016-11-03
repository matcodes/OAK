using System;
using System.Collections.Generic;
using System.Text;

namespace Oak.Services
{
    #region IScannerServiceListener
    public interface IScannerServiceListener
    {
        void ScanProgress(double progress);
    }
    #endregion
}
