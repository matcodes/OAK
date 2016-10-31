using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Oak.Services
{
    #region IScannerService
    public interface IScannerService
    {
        void FindDevice();

        bool Connect();

        string Scan();

        int Timeout { get; set; }
    }
    #endregion
}
