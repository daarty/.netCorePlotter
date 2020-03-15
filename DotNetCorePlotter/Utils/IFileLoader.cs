using System.Collections.Generic;
using OxyPlot;

namespace DotNetCorePlotter.Utils
{
    public interface IFileLoader
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>An array of DataPoints; If the process fails, returns an empty array.</returns>
        List<DataPoint> DisplayDialogueAndLoadDataPoints();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <throws><see cref="LoadDataException"/> if loading the file or parsing it fails.</throws>
        List<DataPoint> LoadFile(string filePath);
    }
}