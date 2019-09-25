using System;
namespace SynaptikonFramework.Util.Math
{
    /// <summary>
    /// Linear function vo storing geometrical function formula in a format y = a0 + a1 * x + a2 * Math.pow(x,2) + a3 * Math.pow(x,3) + ...
    /// In this case y = a1x + a0;
    /// </summary>
    public class LinearFunctionVO
    {
        public LinearFunctionVO(double a0, double a1)
        {
            A0 = a0;
            A1 = a1;
        }

        public double A0 { get; private set; }
        public double A1 { get; private set; }

        public double GetValueAt(double x){
            return A0 + A1 * x;
        }
    }
}
