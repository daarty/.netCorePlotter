using MathNet.Numerics;

namespace DotNetCorePlotter.Utils
{
    /// <summary>
    /// Implementation of the <see cref="IMathHelper"/> interface.
    /// </summary>
    public class MathHelper : IMathHelper
    {
        /// <inheritdoc/>
        public (double a, double b) FindExponentialFunction(double[] xData, double[] yData)
        {
            var tuple = Fit.Exponential(xData, yData);
            return (tuple.Item1, tuple.Item2);
        }

        /// <inheritdoc/>
        public (double a, double b) FindLinearFunction(double[] xData, double[] yData)
        {
            var tuple = Fit.Line(xData, yData);

            // Reversed arguments to maintain the format, because the library returns 'y = a + (b * x)'.
            return (tuple.Item2, tuple.Item1);
        }

        /// <inheritdoc/>
        public (double a, double b) FindPowerFunction(double[] xData, double[] yData)
        {
            var tuple = Fit.Power(xData, yData);
            return (tuple.Item1, tuple.Item2);
        }
    }
}