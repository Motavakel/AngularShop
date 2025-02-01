namespace Application.Wrappers;

public abstract class RequestParametersBasic : PaginationParametersDto
{
    
    public string Search {  get; set; }
    public SortOptions TypeSort { get; set; } = SortOptions.Newest;
}

public enum SortOptions
{
    Newest = 1,       // جدیدترین ها
    PriceHighToLow,   // گران‌ترین
    PriceLowToHigh,   // ارزان‌ترین
    NameAToZ,         // براساس الفبا
}