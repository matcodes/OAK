using System;
using System.Collections.Generic;
using System.Text;

namespace Oak.Services
{
    #region IOakCrossCorr
    public interface IOakCrossCorr
    {
        // First Derivative Absolute Value
        double FDAV(double[] sample1, double[] sample2);

        // First Derivative Least Squares
        double FDLS(double[] sample1, double[] sample2);

        // Absolute Difference Value
        double ADV(double[] sample1, double[] sample2);

        // Least Squares
        double LS(double[] sample1, double[] sample2);

        // Integral method
        double IS(double[] sample1, double[] sample2);

        // Average Line Method
        double SAVG(double[] sample1, double[] sample2);

        // Integral ABS Difference Method
        double IDIFF(double[] sample1, double[] sample2);



        double GetCoeff(double[] sample1, double[] sample2);
    }
    #endregion
}
