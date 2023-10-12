using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Stocker.Parts.Lenovo;

public class ApiCaller
{
    public static async Task<List<ParsedObject>?> GetParts(string fullUrl)
    {
        var apiResponse = await RequestParts(fullUrl);
        if (apiResponse is null) return null;

        return await Deserialize(apiResponse);
    }

    enum PartsListType
    {
        AsBuilt,
        Model,
        Compatible
    }

    public static async Task<string?> RequestParts(string fullUrl)
    {
        var lastPath = new Uri(fullUrl).Segments.LastOrDefault();

        var split = Regex.Match(fullUrl, @"(.*\/|-)(.{4}(\/.*)?)\/parts?").Groups[2].Value.Split('/');
        if (split.Count() == 0) return null;

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://pcsupport.lenovo.com/ee/en/api/v4/upsellAggregation/parts/" + lastPath);

        request.Content = new StringContent(@"{""serialId"":""" + split.ElementAtOrDefault(2) + @""",
                                               ""model"":""" + split.ElementAtOrDefault(1) + @""",
                                               ""mtId"":""" + split.ElementAtOrDefault(0) + @"""}",
                                            Encoding.UTF8,
                                            "application/json");

        try {
            var response = await client.SendAsync(request);
            System.Diagnostics.Debug.WriteLine($@"Response from Lenovo parts list query :: {response}");

            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while performing Lenovo parts list query :: {ex.Message}{Environment.NewLine}{ex.InnerException}");
        }

        return null;
    }

    public static async Task<List<ParsedObject>?> Deserialize(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return null;

        Root? responseObject = null;

        try {
            responseObject = JsonSerializer.Deserialize<Root>(content);

        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while deserializing parts list :: {ex.Message} &-& {ex.InnerException}");
        }

        if (responseObject is null || responseObject.data is null) return null;

        var allProducts = new List<ParsedObject>();

        foreach (var product in responseObject.data) {
            var newProduct = new ParsedObject();

            newProduct.Category = product.commodityVal;
            newProduct.CompatibleModels = product.products.ToList();
            newProduct.Description = product.name;
            newProduct.ExternalIDs = product.externalIds.ToList();
            newProduct.ImageURLs = product.imageUrls.ToList();
            newProduct.PartNumber = product.id;
            newProduct.Substitutes = product.substitutes.Select(x => x.id).ToList();

            allProducts.Add(newProduct);
        }

        return allProducts;
    }
}


