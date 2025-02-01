using Application.Contracts;
using Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Application.Common.BehavioursPipes;

public class CachedQueryBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheQuery, IRequest<TResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private int _hoursSaveData = 2;

    public CachedQueryBehaviour(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
    {
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response;
        var key = GenerateKey();

        var cachedResponse = await _cache.GetAsync(key, cancellationToken);

        if (cachedResponse != null)
            //تبدیل آرایه ای از بایت  ها به جی سان  و در نهایت دی سریالایز
            response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
        else
        {
            response = await next();

            //تبدیل پاسخ(آبجکت) به رشته جی سان و در ادامه تبدیل به آرایه ایی از بایت ها برای ساخت کش توزیع شده
            var serialized = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
            await CreateNewCache(request, key, cancellationToken, serialized);
        }

        return response;
    }

    private string GenerateKey()
    {
        return IdGenerator.GenerateCacheKeyFromRequest(_httpContextAccessor.HttpContext.Request);
    }


    private Task CreateNewCache(TRequest request, string key, CancellationToken cancellationToken, byte[] serialized)
    {
        return _cache.SetAsync(key, serialized,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =new TimeSpan(_hoursSaveData, 0, 0, 0)
            },
            cancellationToken);
    }

}