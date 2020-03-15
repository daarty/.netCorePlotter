using System.Collections.Generic;
using OxyPlot;

namespace DotNetCorePlotter.Utils
{
    /// <summary>
    /// Interface with methods that help loading files.
    /// </summary>
    public interface IFileLoader
    {
        /// <summary>
        /// Displays an 'Open File' dialogue, loads the selected file, parses its content and returns data points.
        /// </summary>
        /// <returns>An array of DataPoints; If the process fails, returns an empty array.</returns>
        List<DataPoint> DisplayDialogueAndLoadDataPoints();
    }
}