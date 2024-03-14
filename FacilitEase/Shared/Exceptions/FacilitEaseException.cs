using FacilitEase.Shared.Enums;
namespace FacilitEase.Shared.Exceptions
{
    [Serializable]
    public sealed class FacilitEaseException :Exception
    {
        public FacilitEaseException()
        { }

        public FacilitEaseException(string message) : base(message)
        { }

        public FacilitEaseException(string message, Exception innerException)
            : base(message, innerException)
        {
            ExceptionMessage = message;
        }

        public FacilitEaseException(int code, string message)
            : base(message)
        {
            ExceptionCode = code;
            ExceptionMessage = message;
        }

        public FacilitEaseException(int code, string message, Exception innerException)
            : base(message, innerException)
        {
            ExceptionCode = code;
            ExceptionMessage = message;
        }

        public FacilitEaseException(ExceptionCode code, string message)
            : base(message)
        {
            ExceptionCode = (int)code;
            ExceptionMessage = message;
        }

        public FacilitEaseException(ExceptionCode code, string message, Exception innerException)
            : base(message, innerException)
        {
            ExceptionCode = (int)code;
            ExceptionMessage = message;
        }
        public int ExceptionCode { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
