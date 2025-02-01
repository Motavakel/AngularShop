using Domain.Entities.ProductEntity;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.SeedData;

public class GenerateFakeData
{


    public static async Task SeedDataAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!await context.ProductBrand.AnyAsync())
                await SeedBrandsAsync(context);

            if (!await context.ProductType.AnyAsync())
                await SeedTypesAsync(context);

            if (!await context.Products.AnyAsync())
                await SeedProductsAsync(context);
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<GenerateFakeData>();
            logger.LogError(e, "Error in seed data");
        }
    }

    private static async Task SeedBrandsAsync(ApplicationDbContext context)
    {
        var brands = new List<ProductBrand>
        {
            new ProductBrand { Title = "لاودیس" },
            new ProductBrand { Title = "جیمز" },
            new ProductBrand { Title = "المپیا" },
        };

        await context.ProductBrand.AddRangeAsync(brands);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTypesAsync(ApplicationDbContext context)
    {
        var types = new List<ProductType>
        {
            new ProductType { Title = "کودک" },
            new ProductType { Title = "کوهستان" },
            new ProductType { Title = "شهری" },
        };

        await context.ProductType.AddRangeAsync(types);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        var products = new List<Product>
        {
            new Product
            {
                Title = "دوچرخه کد 16135-3 سایز 16",
                Description = "دوچرخه‌ها ما را به روزهای کودکی می‌برند، همان لحظاتی که تعادل، تنها دغدغه‌مان بود.",
                Summary = "",
                PictureUrl = "im1.webp",
                Price = 4900000,
                ProductBrandId = 1,
                ProductTypeId = 1,
            },
            new Product
            {
                Title = "دوچرخه کد 16135-1 سایز 16",
                Description = "رکاب بزن، سرعت بگیر و بگذار مسیر خودش را به تو نشان دهد، این است لذت ناب دوچرخه‌سواری!",
                Summary = "",
                PictureUrl = "im2.webp",
                Price = 7900000,
                ProductBrandId = 1,
                ProductTypeId = 1,
            },
            new Product
            {
                Title = "دوچرخه  مدل FOX-D0",
                Description = "دوچرخه‌سواری یعنی استقلال، یعنی توانایی پیمودن مسیرها بدون وابستگی به بنزین و ترافیک.",
                Summary = "",
                PictureUrl = "im3.webp",
                Price = 8900000,
                ProductBrandId = 2,
                ProductTypeId = 1,
            },
            new Product
            {
                Title = "دوچرخه مدل سایز طوقه 24",
                Description = "دوچرخه دوست خوبی برای سفرهای کوتاه و طولانی است، بی‌ادعا ولی همیشه آماده همراهی.",
                Summary = "",
                PictureUrl = "im4.webp",
                Price = 10000000,
                ProductBrandId = 1,
                ProductTypeId = 2,
            },
            new Product
            {
                Title = "دوچرخه هیدرولیکی سايز طوقه 26",
                Description = "روی دوچرخه که باشی، انگار زمین کوچک‌تر و آسمان وسیع‌تر می‌شود، آزادی یعنی همین!",
                Summary = "",
                PictureUrl = "im5.webp",
                Price = 9800000,
                ProductBrandId = 2,
                ProductTypeId = 3,
            },
            new Product
            {
                Title = "دوچرخه مدل GL12 سایز طوقه 12",
                Description = "شاید دوچرخه فقط دو چرخ و یک بدنه باشد، اما می‌تواند تو را به جاهایی ببرد که هیچ ماشینی نمی‌تواند.",
                Summary = "",
                PictureUrl = "im6.webp",
                Price = 25000000,
                ProductBrandId = 1,
                ProductTypeId = 1,
            },
            new Product
            {
                Title = "دوچرخه مدل ultra7 سایز طوقه 26",
                Description = "هیچ چیز لذت‌بخش‌تر از دوچرخه‌سواری در جاده‌ای خلوت، همراه با موسیقی طبیعت نیست.",
                Summary = "",
                PictureUrl = "im7.webp",
                Price = 10000000,
                ProductBrandId = 3,
                ProductTypeId = 2,
            },new Product
            {
                Title = "دوچرخه مدل 12002 سایز 12",
                Description = "دوچرخه‌سواری ورزشی است که هم قلب را قوی‌تر می‌کند و هم زمین را از آلودگی نجات می‌دهد.",
                Summary = "",
                PictureUrl = "im8.webp",
                Price = 7800000,
                ProductBrandId = 1,
                ProductTypeId = 1,
            },new Product
            {
                Title = "دوچرخه مدل MAXXI سایز 20",
                Description = "با هر چرخشی از پدال دوچرخه، نسیم تازه‌ای روی صورتت حس می‌کنی و دنیا از زاویه‌ای جدید دیده می‌شود.",
                Summary = "",
                PictureUrl = "im9.webp",
                Price = 9800000,
                ProductBrandId = 3,
                ProductTypeId = 3,
            }, new Product
            {
                Title = "دوچرخه مدل دیسکی سایز 26",
                Description = "رکاب زدن با دوچرخه نه‌تنها سفر را دل‌نشین‌تر می‌کند، بلکه روح را از هیاهوی روزمره رها می‌سازد.",
                Summary = "",
                PictureUrl = "im10.webp",
                Price = 7800000,
                ProductBrandId = 2,
                ProductTypeId = 2,
            }
            , new Product
            {
                Title = "دوچرخه دینو مدل 27.5",
                Description = "دوچرخه‌ها بهترین همراه ماجراجویی در خیابان‌های شهر و جاده‌های کوهستانی‌اند، بی‌نیاز از سوخت و پر از آزادی.",
                Summary = "",
                PictureUrl = "im11.webp",
                Price = 17800000,
                ProductBrandId = 3,
                ProductTypeId = 3,
            },
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
