using Application.Features.Products.Queries.GetAll;
using FluentValidation;

public class GetAllProductsValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsValidator()
    {

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("تعداد آیتم‌ها باید بزرگتر از ۰ باشد.")
            .LessThanOrEqualTo(50).WithMessage("تعداد آیتم‌ها باید کمتر یا مساوی ۵۰ باشد.");

        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("شماره صفحه باید بزرگتر از ۰ باشد.");

        RuleFor(x => x.Search)
            .MaximumLength(100).WithMessage("طول عبارت جستجو نباید بیشتر از ۱۰۰ کاراکتر باشد.")
            .When(x => !string.IsNullOrEmpty(x.Search)); 

        RuleFor(x => x.TypeSort)
            .IsInEnum().WithMessage("نوع مرتب‌سازی باید یا 'صعودی' یا 'نزولی' باشد.");

        RuleFor(x => x.BrandId)
            .GreaterThanOrEqualTo(0).WithMessage("شناسه برند نمی‌تواند مقدار منفی باشد.");

        RuleFor(x => x.TypeId)
            .GreaterThanOrEqualTo(0).WithMessage("شناسه تایپ نمی‌تواند مقدار منفی باشد.");
    }
}
