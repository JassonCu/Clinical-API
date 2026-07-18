namespace Clinical.Web.Core.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public IEnumerable<object>? Errors { get; set; }
}

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public IEnumerable<object>? Errors { get; set; }
}
