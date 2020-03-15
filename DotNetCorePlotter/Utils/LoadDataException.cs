﻿using System;
using System.Runtime.Serialization;

namespace DotNetCorePlotter.Utils
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [Serializable]
    public class LoadDataException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        public LoadDataException()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="message">Message that describes the error.</param>
        public LoadDataException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="message">Message that describes the error.</param>
        /// <param name="innerException">Exception that caused this exception.</param>
        public LoadDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="info">Serialization information data about the thrown exception.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        private LoadDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}