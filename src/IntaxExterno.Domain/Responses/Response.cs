namespace IntaxExterno.Domain.Responses;

public class Response<T>
{
    public int StatusHttp { get; set; }

    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Object { get; set; }

    public Response() { }

    public Response(bool success)
    {
        Success = success;
    }

    public Response(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public Response(bool success, T? objeto)
    {
        Success = success;
        Object = objeto;
    }

    public Response(bool success, string message, T? objeto)
    {
        Success = success;
        Message = message;
        Object = objeto;
    }

    public Response(bool success, string message, int statusCode)
    {
        Success = success;
        Message = message;
        StatusHttp = statusCode;
    }

    public Response(bool success, string message, T? objeto, int statusHttp)
    {
        Success = success;
        Message = message;
        Object = objeto;
        StatusHttp = statusHttp;
    }
}
