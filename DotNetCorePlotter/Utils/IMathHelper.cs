namespace DotNetCorePlotter.Utils
{
    public interface IMathHelper
    {
        public (double a, double b) FindExponentialFunction(double[] xData, double[] yData);

        public (double a, double b) FindLinearFunction(double[] xData, double[] yData);

        public (double a, double b) FindPowerFunction(double[] xData, double[] yData);
    }
}