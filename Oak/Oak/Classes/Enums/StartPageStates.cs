using System;
using System.Collections.Generic;
using System.Text;

namespace Oak.Classes.Enums
{
    #region StartPageStates
    public enum StartPageStates
    {
        Starting = 0,
        WaitConnection = 1,
        Connecting = 2,
        Connected = 3,
        SelectProduct = 4,
        CameraHelp = 5,
        Camera = 6,
        Scan = 7,
        Store = 8,
        Keep = 9,
        Programs = 10,
        Rescan = 11,
        Compare = 12,
        Check = 13
    }
    #endregion
}
