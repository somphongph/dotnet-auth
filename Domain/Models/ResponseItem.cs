namespace Domain.Models;

public class ResponseItem<T> : BaseResponse
{
    public bool IsCached { get; set; }
    public T? Data { get; set; }
}
