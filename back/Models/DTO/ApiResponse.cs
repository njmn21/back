namespace back.Models.DTO;

public class ApiResponse
{
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = string.Empty;
    public bool Success => StatusCode >= 200 && StatusCode < 300;
}

public class ApiResponse<T> : ApiResponse
{
    public T? Result { get; set; }
}