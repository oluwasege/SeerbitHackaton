namespace SeerbitHackaton.Core.Exceptions
{
    [Serializable]
    public class AppGenericException : Exception
    {
        public string ErrorCode { get; set; }

        public AppGenericException(string message) : base(message)
        { }

        public AppGenericException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}