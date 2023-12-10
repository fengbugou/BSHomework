namespace BSHomework.Exceptions;

public class HttpResponseException : Exception
{
    public int Status;

    public HttpResponseException(int status, string message) : base(message)
    {
        Status = status;
    }
}