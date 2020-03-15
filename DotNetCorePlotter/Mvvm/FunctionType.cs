namespace DotNetCorePlotter.Mvvm
{
    /// <summary>
    /// Specifies different types of functions.
    /// </summary>
    public enum FunctionType
    {
        /// <summary>
        /// Linear function of the type 'y = (a * x) + b'.
        /// </summary>
        Linear,

        /// <summary>
        /// Linear function of the type 'y = a * exp (b * x)'.
        /// </summary>
        Exponential,

        /// <summary>
        /// Linear function of the type 'y = a * (x ^ b)'.
        /// </summary>
        PowerFunction
    }
}