using MathNet.Numerics;

namespace DotNetCorePlotter.Utils
{
    public class MathHelper : IMathHelper
    {
        public (double a, double b) FindLinearFunction(double[] xData, double[] yData)
        {
            var tuple = Fit.Line(xData, yData);

            // Reversed arguments to keep the format:
            //  'y = (a * x) + b'
            //Exponential: 'y = a * exp(b * x)'
            //Power function: 'y = a * (x ^ b)'
            return (tuple.Item2, tuple.Item1);
        }
    }
}