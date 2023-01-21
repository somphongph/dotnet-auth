namespace Domain.Models;

public class ResponseCommand<T> : BaseResponse
{
    public T? Data { get; set; }
}
