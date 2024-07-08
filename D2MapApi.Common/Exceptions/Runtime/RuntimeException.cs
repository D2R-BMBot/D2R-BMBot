using D2MapApi.Server.Grpc.Enumerations.Errors;

namespace D2MapApi.Common.Exceptions.Runtime;

public class RuntimeException : Exception
{
    public RuntimeException() { }
    public RuntimeException(RuntimeErrorCode p_errorCode) => ErrorCode = p_errorCode;
    public RuntimeException(string p_message) : base(p_message) { }
    public RuntimeException(string p_message, Exception p_inner) : base(p_message, p_inner) { }
    
    public RuntimeErrorCode ErrorCode { get; set; }
}