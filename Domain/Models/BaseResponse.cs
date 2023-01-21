namespace Domain.Models;

public abstract class BaseResponse
{
    public bool IsSuccess { get; set; }
    public string? Code { get; set; }
    public string? Message { get; set; }
}
