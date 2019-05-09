using System;

namespace JSGridModels.Exceptions
{
    [Serializable]
    internal class ColumnNotAcceptableException : Exception
    {
        public ColumnNotAcceptableException(string message) : base(message)
        {
        }
    }
}