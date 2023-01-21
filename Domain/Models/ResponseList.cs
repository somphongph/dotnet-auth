namespace Domain.Models;

public class ResponseList<T> : BaseResponse
{
    public bool IsCached { get; set; }
    public IEnumerable<T>? Data { get; set; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}
