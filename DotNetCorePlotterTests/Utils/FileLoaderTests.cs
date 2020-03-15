using System.Collections.Generic;
using System.Reflection;
using DotNetCorePlotter.Utils;
using NUnit.Framework;
using OxyPlot;

namespace DotNetCorePlotterTests.Utils
{
    public class FileLoaderTests
    {
        [Test]
        public void LoadFileJustText()
        {
            var fileLoader = new FileLoader();

            var privateLoadFileMethod = this.GetPrivateMethod(fileLoader, "LoadFile");
            var exception = Assert.Throws<TargetInvocationException>(
                () => privateLoadFileMethod.Invoke(fileLoader, new object[] { TestContext.CurrentContext.WorkDirectory + "\\TestData\\wrong.txt" }));

            Assert.IsTrue(exception.InnerException is LoadDataException);

            var loadDataException = exception.InnerException as LoadDataException;
            Assert.AreEqual(LoadDataErrorCode.InvalidNumberOfValues, loadDataException.ErrorCode);
        }

        [Test]
        public void LoadFileLetterValue()
        {
            var fileLoader = new FileLoader();

            var privateLoadFileMethod = this.GetPrivateMethod(fileLoader, "LoadFile");
            var exception = Assert.Throws<TargetInvocationException>(
                () => privateLoadFileMethod.Invoke(fileLoader, new object[] { TestContext.CurrentContext.WorkDirectory + "\\TestData\\wrong3.txt" }));

            Assert.IsTrue(exception.InnerException is LoadDataException);

            var loadDataException = exception.InnerException as LoadDataException;
            Assert.AreEqual(LoadDataErrorCode.InvalidValue, loadDataException.ErrorCode);
        }

        [Test]
        public void LoadFileNoData()
        {
            var fileLoader = new FileLoader();

            var privateLoadFileMethod = this.GetPrivateMethod(fileLoader, "LoadFile");
            var result =
                privateLoadFileMethod.Invoke(fileLoader, new object[] { TestContext.CurrentContext.WorkDirectory + "\\TestData\\data0.txt" });

            Assert.IsTrue(result is List<DataPoint>);

            var dataPoints = result as List<DataPoint>;
            Assert.AreEqual(0, dataPoints.Count);
        }

        [Test]
        public void LoadFileThatDoesntExist()
        {
            var fileLoader = new FileLoader();

            var privateLoadFileMethod = this.GetPrivateMethod(fileLoader, "LoadFile");
            var exception = Assert.Throws<TargetInvocationException>(
                () => privateLoadFileMethod.Invoke(fileLoader, new object[] { TestContext.CurrentContext.WorkDirectory + "\\TestData\\noSuchFile.txt" }));

            Assert.IsTrue(exception.InnerException is LoadDataException);

            var loadDataException = exception.InnerException as LoadDataException;
            Assert.AreEqual(LoadDataErrorCode.FailedLoadingFile, loadDataException.ErrorCode);
        }

        [Test]
        public void LoadFileThreeValuesInLine()
        {
            var fileLoader = new FileLoader();

            var privateLoadFileMethod = this.GetPrivateMethod(fileLoader, "LoadFile");
            var exception = Assert.Throws<TargetInvocationException>(
                () => privateLoadFileMethod.Invoke(fileLoader, new object[] { TestContext.CurrentContext.WorkDirectory + "\\TestData\\wrong2.txt" }));

            Assert.IsTrue(exception.InnerException is LoadDataException);

            var loadDataException = exception.InnerException as LoadDataException;
            Assert.AreEqual(LoadDataErrorCode.InvalidNumberOfValues, loadDataException.ErrorCode);
        }

        [Test]
        public void LoadFileValidData()
        {
            var fileLoader = new FileLoader();

            var privateLoadFileMethod = this.GetPrivateMethod(fileLoader, "LoadFile");
            var result =
                privateLoadFileMethod.Invoke(fileLoader, new object[] { TestContext.CurrentContext.WorkDirectory + "\\TestData\\data.txt" });

            Assert.IsTrue(result is List<DataPoint>);

            var dataPoints = result as List<DataPoint>;

            Assert.AreEqual(25, dataPoints.Count);
            Assert.AreEqual(new DataPoint(0d, 0d), dataPoints[0]);
            Assert.AreEqual(new DataPoint(10000d, 200d), dataPoints[1]);
            Assert.AreEqual(new DataPoint(3d, 3d), dataPoints[2]);
            Assert.AreEqual(new DataPoint(-1d, -1d), dataPoints[24]);
        }

        private MethodInfo GetPrivateMethod(FileLoader subject, string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace");
            }

            var method = subject.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }

        //private Mock<IFileLoader> fileLoaderMock;
        //private Mock<IMathHelper> mathHelperMock;

        //[Test]
        //public void LoadFile()
        //{
        //    var viewModel = new MainViewModel(fileLoaderMock.Object, mathHelperMock.Object);

        //    Assert.NotNull(viewModel.LoadFileCommand);
        //    Assert.NotNull(viewModel.FindFunctionCommand);
        //    Assert.NotNull(viewModel.DrawFunctionCommand);
        //    Assert.AreEqual(FunctionType.Linear, viewModel.FunctionType);
        //    Assert.AreEqual(0, viewModel.DataPoints.Count);
        //    Assert.AreEqual(0, viewModel.GeneratedDataPoints.Count);
        //    Assert.IsFalse(viewModel.IsValidPlotLoaded);
        //    Assert.AreEqual("y = (0 * x) + 0", viewModel.ResultingFunction);
        //    Assert.AreEqual("0", viewModel.VariableA);
        //    Assert.AreEqual("0", viewModel.VariableB);
        //}

        //[SetUp]
        //public void Setup()
        //{
        //    this.fileLoaderMock = new Mock<IFileLoader>();
        //    this.mathHelperMock = new Mock<IMathHelper>();
        //}
    }
}