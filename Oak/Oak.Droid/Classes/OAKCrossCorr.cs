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
using Oak.Droid.Classes;
using Xamarin.Forms;
using Oak.Services;

[assembly: Dependency(typeof(OAKCrossCorr))]
namespace Oak.Droid.Classes
{
    #region OAKCrossCorr
    public class OAKCrossCorr : IOakCrossCorr
    {
        // First Derivative Absolute Value
        public double FDAV(double[] sample1, double[] sample2)
        {
            double result = 0.0;
            double n = 0.0;
            double d = 0.0;

            for (int i = 1; i < sample1.Length - 2; i++)
            {
                var ds = sample1[i] - sample1[i - 1];
                var dr = sample2[i] - sample1[i - 1];
                n += Math.Abs(ds);
                d += Math.Abs(ds - dr);
            }

            result = (1 - n / d) * 100;

            return result;
        }

        // First Derivative Least Squares
        public double FDLS(double[] sample1, double[] sample2)
        {
            double result = 0.0;
            double n = 0.0;
            double d = 0.0;

            for (int i = 1; i < sample1.Length - 2; i++)
            {
                var ds = sample1[i] - sample1[i - 1];
                var dr = sample2[i] - sample1[i - 1];
                n += ds * ds;
                d += (ds - dr) * (ds - dr);
            }

            result = (1 - n / d) * 100;

            return result;
        }

        // Absolute Difference Value
        public double ADV(double[] sample1, double[] sample2)
        {
            double result = 0.0;
            double n = 0.0;
            double d = 0.0;

            for (int i = 1; i < sample1.Length - 2; i++)
            {
                n += Math.Abs(sample1[i]);
                d += Math.Abs(sample1[i] - sample2[i]);
            }

            result = (1 - n / d) * 100;

            return result;
        }

        // Least Squares
        public double LS(double[] sample1, double[] sample2)
        {
            double result = 0.0;
            double n = 0.0;
            double d = 0.0;

            for (int i = 1; i < sample1.Length - 2; i++)
            {
                d += (sample1[i] - sample2[i]) * (sample1[i] - sample2[i]);
                n += sample1[i] * sample1[i];
            }

            result = (1 - n / d) * 100;

            return result;
        }

        // Integral method
        public double IS(double[] sample1, double[] sample2)
        {
            double result = 0.0;
            double n = 0.0;
            double d = 0.0;
            double temp = 65535;

            temp = sample1.Skip(1).Take(sample1.Length - 2).Where(a => a < temp).Min();

            for (int i = 1; i < sample1.Length - 2; i++)
            {
                d += sample1[i] - temp;
                n += sample2[i] - temp;
            }

            result = (1 - Math.Abs(n - d) / n) * 100;

            return result;
        }

        // Average Line Method
        public double SAVG(double[] sample1, double[] sample2)
        {
            double result = 0.0;
            double n = 0.0;
            double d = 0.0;

            for (int i = 1; i < sample1.Length - 2; i++)
            {
                d += sample1[i];
                n += sample2[i];
            }

            d += d / sample1.Length;
            n += n / sample1.Length;

            result = Math.Abs(n - d) / d * 100;

            return result;
        }

        // Integral ABS Difference Method
        public double IDIFF(double[] sample1, double[] sample2)
        {
            double result = 0.0;
            double d = 0.0;

            for (int i = 50; i < 1000; i++)
                d += (sample1[i] - sample2[i]);

            result = Math.Abs(d);

            return result;
        }

        public double GetCoeff(double[] sample1, double[] sample2)
        {
            SimpleRegression regression = new SimpleRegression();
            for (int i = 0; i < sample1.Length; i++)
                regression.AddData(sample1[i], sample2[i]);
            return regression.GetRegression();
        }
    }
    #endregion
}