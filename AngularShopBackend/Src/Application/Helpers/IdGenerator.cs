using System.Text;

namespace Application.Helpers;

using Microsoft.AspNetCore.Http;

public class IdGenerator
{
    public static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"{request.Path}"); 

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            keyBuilder.Append($"|{key}-{value}");

        return keyBuilder.ToString();
    }
}

/*
 یک توضیح

http://localhost:9001/api/products?brandId=2&typeId=5&search=test
 
request.Path = api/products
request.Query = products?brandId=2&typeId=5&search=test 
 
 
 */