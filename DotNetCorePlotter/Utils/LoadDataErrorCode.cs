namespace DotNetCorePlotter.Utils
{
    /// <summary>
    /// Specifies different types of error codes during the loading of data.
    /// </summary>
    public enum LoadDataErrorCode
    {
        /// <summary>
        /// Failed loading the file.
        /// Possible reasons: no access rights, file not available.
        /// </summary>
        FailedLoadingFile,

        /// <summary>
        /// Invalid number of values in one line.
        /// Every line must contain two values.
        /// </summary>
        InvalidNumberOfValues,

        /// <summary>
        /// Invalid value.
        /// The program was not able to parse the value.
        /// </summary>
        InvalidValue
    }
}