using Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Common.BehavioursPipes;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    //تعریف لیستی از اعتبار سنجی برای هر درخواست 
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        //اگر اعتبارسنجی وجود نداشت برو به مرحله بعد 
        if (!_validators.Any()) return await next();

        //برای هریک از اعتبارسنجی ها با استفاده از ولیدیت ای سینک و نسخه کامل  درخواست ، اعتبارسنجی رو انجام میدهمی
        var validationResults = 
        await Task.WhenAll(_validators
        .Select(v => v.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken)));
        
        //جداسازی خطاها و پرتاپ استثنا در صورت وجود
        var failures = validationResults.SelectMany(r => r.Errors).ToList();

        if (failures.Any()) throw new ValidationEntityException(failures);

        //رفتن به مرحله بعد
        return await next();
    }
}


/*
یک نکته در مورد ورودی دلیگیت 
همانطور که م یدانیم دلیگیت ها به متد ها اشاره می کنند

در اینجا هم هر متدی که امضای دلیگیت مورد نظر رو رعایت کنه در مرحله بعدی 
و توسط ترتیبی که ما تعیین می کنیم که در کلاس  کانفیگور سرویس این لایه 
قایل مشاهده است ، فراخوان و اجرا می شود
 
 */