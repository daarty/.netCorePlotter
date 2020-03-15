using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using OxyPlot;

namespace DotNetCorePlotter.Utils
{
    /// <summary>
    /// Implementation of the <see cref="IFileLoader"/> interface.
    /// </summary>
    public class FileLoader : IFileLoader
    {
        /// <inheritdoc/>
        public List<DataPoint> DisplayDialogueAndLoadDataPoints()
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Select Data File";
            dialog.Filter = "All files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            var dataPoints = new List<DataPoint> { };
            var result = dialog.ShowDialog();
            if (result ?? false)
            {
                try
                {
                    dataPoints = this.LoadFile(dialog.FileName);
                }
                catch (LoadDataException ex)
                {
                    Debug.WriteLine($"Failed loading file '{dialog.FileName}'. Cause: '{ex.Message}'");
                }
            }

            if (!dataPoints.Any())
            {
                return dataPoints;
            }

            try
            {
                dataPoints.Sort((p1, p2) => p1.X.CompareTo(p2.X));
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
            {
                Debug.WriteLine($"Failed sorting Datapoints. Cause: '{ex.Message}'");
                dataPoints = new List<DataPoint> { };
            }

            return dataPoints;
        }

        private List<DataPoint> LoadFile(string filePath)
        {
            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch (Exception ex)
            {
                throw new LoadDataException($"Failed loading the file. Cause: '{ex.Message}'");
            }

            var data = new List<DataPoint>();

            Debug.WriteLine("Parsing DataPoints ...");
            foreach (var line in lines)
            {
                var values = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length != 2)
                {
                    throw new LoadDataException($"Failed parsing the file because of an invalid format in the following line '{line}'.");
                }

                double x, y = 0d;
                x = ParseDouble(values[0]);
                y = ParseDouble(values[1]);

                Debug.WriteLine($" - x: '{x}', y: '{y}'");
                data.Add(new DataPoint(x, y));
            }

            Debug.WriteLine("Finished parsing DataPoints.");

            return data;
        }

        private double ParseDouble(string input)
        {
            try
            {
                return double.Parse(input, new CultureInfo("en-US"));
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
            {
                throw new LoadDataException($"Failed parsing the value '{input}'. Cause: '{ex.Message}'");
            }
        }
    }
}