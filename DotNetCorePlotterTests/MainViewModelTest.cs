using System.Collections.Generic;
using DotNetCorePlotter;
using DotNetCorePlotter.Mvvm;
using DotNetCorePlotter.Utils;
using Moq;
using NUnit.Framework;
using OxyPlot;

namespace DotNetCorePlotterTests
{
    public class MainViewModelTest
    {
        private Mock<IFileLoader> fileLoaderMock;
        private Mock<IMathHelper> mathHelperMock;

        [Test]
        public void ConstructorAssureAllValues()
        {
            var viewModel = new MainViewModel(fileLoaderMock.Object, mathHelperMock.Object);

            Assert.NotNull(viewModel.LoadFileCommand);
            Assert.NotNull(viewModel.FindFunctionCommand);
            Assert.NotNull(viewModel.DrawFunctionCommand);
            Assert.AreEqual(FunctionType.Linear, viewModel.FunctionType);
            Assert.AreEqual(0, viewModel.DataPoints.Count);
            Assert.AreEqual(0, viewModel.GeneratedDataPoints.Count);
            Assert.IsFalse(viewModel.IsValidPlotLoaded);
            Assert.AreEqual("y = (0 * x) + 0", viewModel.ResultingFunction);
            Assert.AreEqual("0", viewModel.VariableA);
            Assert.AreEqual("0", viewModel.VariableB);
        }

        [Test]
        public void ExecuteDrawFunctionCommandWithNoDataPoints()
        {
            var dataPoints = new List<DataPoint> { };
            fileLoaderMock.Setup(x => x.DisplayDialogAndLoadDataPoints()).Returns(dataPoints);

            var viewModel = new MainViewModel(fileLoaderMock.Object, mathHelperMock.Object);
            viewModel.DrawFunctionCommand.Execute(null);

            fileLoaderMock.Verify(x => x.DisplayDialogAndLoadDataPoints(), Times.Never);
            Assert.AreEqual(0, viewModel.DataPoints.Count);
            Assert.AreEqual(0, viewModel.GeneratedDataPoints.Count);
            Assert.IsFalse(viewModel.IsValidPlotLoaded);
        }

        [Test]
        public void ExecuteFindFunctionCommandWithExponentialFunction()
        {
            var dataPoints = new List<DataPoint> { new DataPoint(2, 4), new DataPoint(5, 5) };
            (double a, double b) tuple = (1.5d, 3d);

            fileLoaderMock.Setup(x => x.DisplayDialogAndLoadDataPoints()).Returns(dataPoints);
            mathHelperMock.Setup(x => x.FindLinearFunction(It.IsAny<double[]>(), It.IsAny<double[]>())).Returns(tuple);
            mathHelperMock.Setup(x => x.FindExponentialFunction(new double[] { 2d, 5d }, new double[] { 4d, 5d })).Returns(tuple);
            mathHelperMock.Setup(x => x.FindPowerFunction(It.IsAny<double[]>(), It.IsAny<double[]>())).Returns(tuple);

            var viewModel = new MainViewModel(fileLoaderMock.Object, mathHelperMock.Object);
            viewModel.FunctionType = FunctionType.Exponential;
            viewModel.LoadFileCommand.Execute(null);

            fileLoaderMock.Verify(x => x.DisplayDialogAndLoadDataPoints(), Times.Once);
            mathHelperMock.Verify(x => x.FindExponentialFunction(new double[] { 2d, 5d }, new double[] { 4d, 5d }), Times.Once);
            Assert.AreEqual(2, viewModel.DataPoints.Count);
            Assert.AreEqual(5000, viewModel.GeneratedDataPoints.Count);

            viewModel.FindFunctionCommand.Execute(null);
            mathHelperMock.Verify(x => x.FindLinearFunction(It.IsAny<double[]>(), It.IsAny<double[]>()), Times.Never());
            mathHelperMock.Verify(x => x.FindExponentialFunction(new double[] { 2d, 5d }, new double[] { 4d, 5d }), Times.Exactly(2));
            mathHelperMock.Verify(x => x.FindPowerFunction(It.IsAny<double[]>(), It.IsAny<double[]>()), Times.Never());
            Assert.AreEqual(5000, viewModel.GeneratedDataPoints.Count);
            Assert.AreEqual("1.5", viewModel.VariableA);
            Assert.AreEqual("3", viewModel.VariableB);
            Assert.AreEqual("y = 1.5 * exp (3 * x)", viewModel.ResultingFunction);
        }

        [Test]
        public void ExecuteFindFunctionCommandWithLinearFunction()
        {
            var dataPoints = new List<DataPoint> { new DataPoint(2, 4), new DataPoint(5, 5) };
            (double a, double b) tuple = (1.5d, 3d);

            fileLoaderMock.Setup(x => x.DisplayDialogAndLoadDataPoints()).Returns(dataPoints);
            mathHelperMock.Setup(x => x.FindLinearFunction(new double[] { 2d, 5d }, new double[] { 4d, 5d })).Returns(tuple);
            mathHelperMock.Setup(x => x.FindExponentialFunction(It.IsAny<double[]>(), It.IsAny<double[]>())).Returns(tuple);
            mathHelperMock.Setup(x => x.FindPowerFunction(It.IsAny<double[]>(), It.IsAny<double[]>())).Returns(tuple);

            var viewModel = new MainViewModel(fileLoaderMock.Object, mathHelperMock.Object);
            viewModel.LoadFileCommand.Execute(null);

            fileLoaderMock.Verify(x => x.DisplayDialogAndLoadDataPoints(), Times.Once);
            mathHelperMock.Verify(x => x.FindLinearFunction(new double[] { 2d, 5d }, new double[] { 4d, 5d }), Times.Once);
            Assert.AreEqual(2, viewModel.DataPoints.Count);
            Assert.AreEqual(5000, viewModel.GeneratedDataPoints.Count);

            viewModel.FindFunctionCommand.Execute(null);
            mathHelperMock.Verify(x => x.FindLinearFunction(new double[] { 2d, 5d }, new double[] { 4d, 5d }), Times.Exactly(2));
            mathHelperMock.Verify(x => x.FindExponentialFunction(It.IsAny<double[]>(), It.IsAny<double[]>()), Times.Never());
            mathHelperMock.Verify(x => x.FindPowerFunction(It.IsAny<double[]>(), It.IsAny<double[]>()), Times.Never());
            Assert.AreEqual(5000, viewModel.GeneratedDataPoints.Count);
            Assert.AreEqual("1.5", viewModel.VariableA);
            Assert.AreEqual("3", viewModel.VariableB);
            Assert.AreEqual("y = (1.5 * x) + 3", viewModel.ResultingFunction);
        }

        [Test]
        public void ExecuteFindFunctionCommandWithNoDataPoints()
        {
            var viewModel = new MainViewModel(fileLoaderMock.Object, mathHelperMock.Object);

            viewModel.FindFunctionCommand.Execute(null);
            Assert.AreEqual(0, viewModel.DataPoints.Count);
            Assert.AreEqual(0, viewModel.GeneratedDataPoints.Count);
        }

        [Test]
        public void ExecuteFindFunctionCommandWithPowerFunction()
        {
            var dataPoints = new List<DataPoint> { new DataPoint(2, 4), new DataPoint(5, 5) };
            (double a, double b) tuple = (1.5d, 3d);

            fileLoaderMock.Setup(x => x.DisplayDialogAndLoadDataPoints()).Returns(dataPoints);
            mathHelperMock.Setup(x => x.FindLinearFunction(It.IsAny<double[]>(), It.IsAny<double[]>())).Returns(tuple);
            mathHelperMock.Setup(x => x.FindExponentialFunction(It.IsAny<double[]>(), It.IsAny<double[]>())).Returns(tuple);
            mathHelperMock.Setup(x => x.FindPowerFunction(new double[] { 2d, 5d }, new double[] { 4d, 5d })).Returns(tuple);

            var viewModel = new MainViewModel(fileLoaderMock.Object, mathHelperMock.Object);
            viewModel.FunctionType = FunctionType.PowerFunction;
            viewModel.LoadFileCommand.Execute(null);

            fileLoaderMock.Verify(x => x.DisplayDialogAndLoadDataPoints(), Times.Once);
            mathHelperMock.Verify(x => x.FindPowerFunction(new double[] { 2d, 5d }, new double[] { 4d, 5d }), Times.Once);
            Assert.AreEqual(2, viewModel.DataPoints.Count);
            Assert.AreEqual(5000, viewModel.GeneratedDataPoints.Count);

            viewModel.FindFunctionCommand.Execute(null);
            mathHelperMock.Verify(x => x.FindLinearFunction(It.IsAny<double[]>(), It.IsAny<double[]>()), Times.Never());
            mathHelperMock.Verify(x => x.FindExponentialFunction(It.IsAny<double[]>(), It.IsAny<double[]>()), Times.Never());
            mathHelperMock.Verify(x => x.FindPowerFunction(new double[] { 2d, 5d }, new double[] { 4d, 5d }), Times.Exactly(2));
            Assert.AreEqual(5000, viewModel.GeneratedDataPoints.Count);
            Assert.AreEqual("1.5", viewModel.VariableA);
            Assert.AreEqual("3", viewModel.VariableB);
            Assert.AreEqual("y = 1.5 * (x ^ 3)", viewModel.ResultingFunction);
        }

        [Test]
        public void ExecuteLoadFileCommand()
        {
            var dataPoints = new List<DataPoint> { new DataPoint(2, 4), new DataPoint(5, 5) };
            fileLoaderMock.Setup(x => x.DisplayDialogAndLoadDataPoints()).Returns(dataPoints);

            var viewModel = new MainViewModel(fileLoaderMock.Object, mathHelperMock.Object);
            viewModel.LoadFileCommand.Execute(null);

            fileLoaderMock.Verify(x => x.DisplayDialogAndLoadDataPoints(), Times.Once);
            Assert.AreEqual(2, viewModel.DataPoints.Count);
            Assert.AreEqual(2d, viewModel.DataPoints[0].X);
            Assert.AreEqual(4d, viewModel.DataPoints[0].Y);
            Assert.AreEqual(5d, viewModel.DataPoints[1].X);
            Assert.AreEqual(5d, viewModel.DataPoints[1].Y);
            Assert.IsTrue(viewModel.IsValidPlotLoaded);
            Assert.AreEqual(5000, viewModel.GeneratedDataPoints.Count);
        }

        [Test]
        public void ExecuteLoadFileCommandWithNoDataPoints()
        {
            var dataPoints = new List<DataPoint> { };
            fileLoaderMock.Setup(x => x.DisplayDialogAndLoadDataPoints()).Returns(dataPoints);

            var viewModel = new MainViewModel(fileLoaderMock.Object, mathHelperMock.Object);
            viewModel.LoadFileCommand.Execute(null);

            fileLoaderMock.Verify(x => x.DisplayDialogAndLoadDataPoints(), Times.Once);
            Assert.AreEqual(0, viewModel.DataPoints.Count);
            Assert.IsFalse(viewModel.IsValidPlotLoaded);
        }

        [SetUp]
        public void Setup()
        {
            this.fileLoaderMock = new Mock<IFileLoader>();
            this.mathHelperMock = new Mock<IMathHelper>();
        }
    }
}