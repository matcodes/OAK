using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace Oak.Droid.Classes
{
    #region SimpleRegression
    public class SimpleRegression
    {
        private double _sumX = 0;
        private double _sumY = 0;

        private double _sumXX = 0;
        private double _sumYY = 0;
        private double _sumXY = 0;

        private long _n = 0;

        private double _xBar = 0;
        private double _yBar = 0;

        private readonly bool _hasIntercept;


        public SimpleRegression() : this(true)
        {
        }

        public SimpleRegression(bool hasIntercept)
        {
            _hasIntercept = hasIntercept;
        }

        public void AddData(double x, double y)
        {
            if (_n == 0)
            {
                _xBar = x;
                _yBar = y;
            }
            else
            {
                if (_hasIntercept)
                {
                    var fact1 = 1.0 + _n;
                    var fact2 = _n / (1.0 + _n);
                    var dx = x - _xBar;
                    var dy = y - _yBar;
                    _sumXX += dx * dx * fact2;
                    _sumYY += dy * dy * fact2;
                    _sumXY += dx * dy * fact2;
                    _xBar += dx / fact1;
                    _yBar += dy / fact1;
                }
            }
            if (!_hasIntercept)
            {
                _sumXX += x * x;
                _sumYY += y * y;
                _sumXY += x * y;
            }
            _sumX += x;
            _sumY += y;
            _n++;
        }

        public double GetRegression()
        {
            double b1 = this.GetSlope();
            double result = Math.Sqrt(getRSquare());
            if (b1 < 0)
            {
                result = -result;
            }
            return result;
        }

        private double GetSlope()
        {
            return _sumXY / _sumXX;
        }

        private double getRSquare()
        {
            var ssto = this.GetTotalSumSquares();
            return (ssto - this.GetSumSquaredErrors()) / ssto;
        }

        public double GetTotalSumSquares()
        {
            return _sumYY;
        }

        private double GetSumSquaredErrors()
        {
            return Math.Max((double)0, _sumYY - _sumXY * _sumXY / _sumXX);
        }
    }
    #endregion
}