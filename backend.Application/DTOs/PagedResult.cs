namespace backend.Application.DTOs;

/// <summary>Generic cursor-style page envelope returned by all paginated endpoints.</summary>
public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int              Page,
    int              PageSize,
    int              TotalCount)
{
    public int  TotalPages  => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNext     => Page < TotalPages;
    public bool HasPrevious => Page > 1;
}
