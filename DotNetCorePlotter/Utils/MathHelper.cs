using MathNet.Numerics;

namespace DotNetCorePlotter.Utils
{
    public class MathHelper : IMathHelper
    {
        public (double a, double b) FindExponentialFunction(double[] xData, double[] yData)
        {
            var tuple = Fit.Exponential(xData, yData);

            // Returning arguments to maintain the format:
            //  'y = a * exp(b * x)'
            return (tuple.Item1, tuple.Item2);
        }

        public (double a, double b) FindLinearFunction(double[] xData, double[] yData)
        {
            var tuple = Fit.Line(xData, yData);

            // Reversed arguments to maintain the format:
            //  'y = (a * x) + b'
            return (tuple.Item2, tuple.Item1);
        }

        public (double a, double b) FindPowerFunction(double[] xData, double[] yData)
        {
            var tuple = Fit.Power(xData, yData);

            // Returning arguments to maintain the format:
            //  'y = a * (x ^ b)'
            return (tuple.Item1, tuple.Item2);
        }
    }
}