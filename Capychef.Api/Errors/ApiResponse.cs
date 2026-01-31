namespace Capychef.Api.Errors;

public class ApiResponse<T>
{
    public ApiResponse(T data)
    {
        Data = data;
    }

    public ApiResponse(ApiError error)
    {
        Error = error;
    }

    public T? Data { get; set; }
    public ApiError? Error { get; set; }
}