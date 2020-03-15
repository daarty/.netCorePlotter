namespace DotNetCorePlotter.Utils
{
    /// <summary>
    /// Interface with methods that help with mathematical functions.
    /// </summary>
    public interface IMathHelper
    {
        /// <summary>
        /// Automatically finds the best suiting exponential function for the given data.
        /// </summary>
        /// <param name="xData">X values of the data points.</param>
        /// <param name="yData">Y values of the data points.</param>
        /// <returns>
        /// A tuple containing a and b of the exponential function 'y = a * exp(b * x)'.
        /// </returns>
        public (double a, double b) FindExponentialFunction(double[] xData, double[] yData);

        /// <summary>
        /// Automatically finds the best suiting linear function for the given data.
        /// </summary>
        /// <param name="xData">X values of the data points.</param>
        /// <param name="yData">Y values of the data points.</param>
        /// <returns>
        /// A tuple containing a and b of the exponential function 'y = (a * x) + b'.
        /// </returns>
        public (double a, double b) FindLinearFunction(double[] xData, double[] yData);

        /// <summary>
        /// Automatically finds the best suiting power function for the given data.
        /// </summary>
        /// <param name="xData">X values of the data points.</param>
        /// <param name="yData">Y values of the data points.</param>
        /// <returns>
        /// A tuple containing a and b of the exponential function 'y = a * (x ^ b)'.
        /// </returns>
        public (double a, double b) FindPowerFunction(double[] xData, double[] yData);
    }
}