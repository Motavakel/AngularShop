namespace Application.Wrappers;

public abstract class PaginationParametersDto
{
    private const int _MaxPageSize = 20;

    private int _pageIndex = 1;
    private int _pageSize = 12;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(value, _MaxPageSize);
    }

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = Math.Max(value, 1);
    }
}
