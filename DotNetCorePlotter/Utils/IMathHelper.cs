namespace DotNetCorePlotter.Utils
{
    public interface IMathHelper
    {
        public (double a, double b) FindLinearFunction(double[] xData, double[] yData);
    }
}