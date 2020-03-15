using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using OxyPlot;

namespace DotNetCorePlotter.Utils
{
    public class FileLoader : IFileLoader
    {
        /// <inheritdoc/>
        public DataPoint[] DisplayDialogueAndLoadDataPoints()
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Select Data File";
            dialog.Filter = "All files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            var result = dialog.ShowDialog();
            if (result ?? false)
            {
                try
                {
                    return this.LoadFile(dialog.FileName);
                }
                catch (LoadDataException ex)
                {
                    Debug.WriteLine($"Failed loading file '{dialog.FileName}'. Cause: '{ex.Message}'");
                }
            }

            return new DataPoint[] { };
        }

        /// <inheritdoc/>
        public DataPoint[] LoadFile(string filePath)
        {
            // TODO Add CancellationToken
            // TODO async ReadAllLinesAsync
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

            return data.ToArray();
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