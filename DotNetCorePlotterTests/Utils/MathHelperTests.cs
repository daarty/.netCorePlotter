using DotNetCorePlotter.Utils;
using NUnit.Framework;

namespace DotNetCorePlotterTests.Utils
{
    public class MathHelperTests
    {
        private MathHelper mathHelper;

        private double[] xData;

        private double[] yData;

        [Test]
        public void FindExponentialFunctionTest()
        {
            var result = mathHelper.FindExponentialFunction(xData, yData);

            Assert.AreEqual(0d, result.a);
            Assert.AreEqual(double.PositiveInfinity, result.b);
        }

        [Test]
        public void FindLinearFunctionTest()
        {
            var result = mathHelper.FindLinearFunction(xData, yData);

            Assert.AreEqual(1d, result.a);
            Assert.AreEqual(0d, result.b);
        }

        [Test]
        public void FindPowerFunctionTest()
        {
            var result = mathHelper.FindPowerFunction(xData, yData);

            Assert.AreEqual(double.NaN, result.a);
            Assert.AreEqual(double.NaN, result.b);
        }

        [SetUp]
        public void Setup()
        {
            this.mathHelper = new MathHelper();
            this.xData = new double[] { 0, 1 };
            this.yData = new double[] { 0, 1 };
        }
    }
}