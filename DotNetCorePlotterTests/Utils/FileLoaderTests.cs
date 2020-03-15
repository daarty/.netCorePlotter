using System.Collections.Generic;
using System.Reflection;
using DotNetCorePlotter.Utils;
using NUnit.Framework;
using OxyPlot;

namespace DotNetCorePlotterTests.Utils
{
    public class FileLoaderTests
    {
        private FileLoader fileLoader;

        [Test]
        public void LoadFileJustText()
        {
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

        [SetUp]
        public void Setup()
        {
            this.fileLoader = new FileLoader();
        }

        [Test]
        public void SortDataPointsWithValidPoints()
        {
            var dataPoints = new List<DataPoint> { new DataPoint(1000, -5), new DataPoint(-1, 5), new DataPoint(5, 1000) };

            var privateLoadFileMethod = this.GetPrivateMethod(fileLoader, "SortDataPoints");
            privateLoadFileMethod.Invoke(fileLoader, new object[] { dataPoints });

            Assert.AreEqual(3, dataPoints.Count);
            Assert.AreEqual(new DataPoint(-1, 5), dataPoints[0]);
            Assert.AreEqual(new DataPoint(5, 1000), dataPoints[1]);
            Assert.AreEqual(new DataPoint(1000, -5), dataPoints[2]);
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
    }
}