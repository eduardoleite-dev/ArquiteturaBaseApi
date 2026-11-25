namespace ArquiteturaBaseApi.DTOs;

public record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    long TotalItems,
    int PageNumber,
    int PageSize
)
{
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;
}
