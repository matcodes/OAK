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

        Task<bool> ConnectAsync();

        int Timeout { get; set; }
    }
    #endregion
}
