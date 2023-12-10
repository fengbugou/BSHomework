namespace BSHomework.Exceptions;

public class HttpResponseException
{
    public int Status;
    public string Message;

    public HttpResponseException(int status, string message)
    {
        Status = status;
        Message = message;
    }
}