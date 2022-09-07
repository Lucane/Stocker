using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
//using System.Windows.Forms;


namespace Stocker.Parts.Lenovo;

public class ApiCaller
{
    public static async Task<List<ParsedObject>?> GetParts(string fullUrl)
    {
        var apiResponse = await RequestParts(fullUrl);
        if (apiResponse is null) return null;

        return await Deserialize(apiResponse);

        //var populateResponse = await PopulateDatabase(apiResponse);
        //if (populateResponse is false) return null;

        //return await Task.FromResult(true);
    }

    enum PartsListType
    {
        AsBuilt,
        Model,
        Compatible
    }

    /*public static async Task<string?> RequestModelParts_old(string fullUrl)
    {
        var lastPath = new Uri(fullUrl).Segments.LastOrDefault();
        //if (lastPath == "as-built") lastPath = "model";

        var split = Regex.Match(fullUrl, @"(.*\/|-)(.{4}(\/.*)?)\/parts?").Groups[2].Value.Split('/');

        if (split.Count() == 0) return null;

        var client = new HttpClient();
        //client.DefaultRequestHeaders.Add("referer", fullUrl);
        //client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
        //client.DefaultRequestHeaders.Host = "pcsupport.lenovo.com";
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://pcsupport.lenovo.com/us/en/api/v4/upsellAggregation/parts/" + lastPath);
        request = new HttpRequestMessage(HttpMethod.Post, "https://pcsupport.lenovo.com/us/en/api/v4/upsellAggregation/parts/compatible");
        request.Headers.Host = "pcsupport.lenovo.com";
        //var request = (HttpWebRequest)WebRequest.Create("https://pcsupport.lenovo.com/us/en/api/v4/upsellAggregation/parts/" + lastPath);
        //request.Method = "POST";
        //request.ContentType = "application/json";
        //var data = @"{""serialId"":""" + split.ElementAtOrDefault(2) + @""",
        //                                       ""model"":""" + split.ElementAtOrDefault(1) + @""",
        //                                       ""mtId"":""" + split.ElementAtOrDefault(0) + @"""}";

        //using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
        //    streamWriter.Write(data);
        //}
        request.Content = new StringContent(@"{""serialId"":""" + split.ElementAtOrDefault(2) + @""",
                                               ""model"":""" + split.ElementAtOrDefault(1) + @""",
                                               ""mtId"":""" + split.ElementAtOrDefault(0) + @"""}",
                                            Encoding.UTF8,
                                            "application/json");

        //request.Content.Headers.Add("Content-Length", (await request.Content.ReadAsStringAsync()).Length.ToString());

        //System.Diagnostics.Debug.WriteLine(await request.Content.ReadAsStringAsync());

        //var list = new List<string>();
        //list.Add("5CB0Z69532");
        //list.Add("5CB0Z69533");
        //return await Flex.RetrieveStockStatus(list);

        try {
            //var httpResponse = (HttpWebResponse)request.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
            //    var result = await streamReader.ReadToEndAsync();

            //}
            var response = await client.SendAsync(request);
            System.Diagnostics.Debug.WriteLine($@"Response from Lenovo PCSupport :: {response}");
            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
            //var response = await client.SendAsync(request);
            //var response = await request.GetResponseAsync();

        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while fetching session storage from Lenovo :: {ex.Message} &-& {ex.InnerException}");
        }

        return null;
    }*/

    //public static async Task<string?> RequestParts(string fullUrl)
    //{
    //    var lastPath = new Uri(fullUrl).Segments.LastOrDefault();

    //    var split = Regex.Match(fullUrl, @"(.*\/|-)(.{4}(\/.*)?)\/parts?").Groups[2].Value.Split('/');
    //    if (split.Count() == 0) return null;

    //    var httpRequest = (HttpWebRequest)WebRequest.Create("https://pcsupport.lenovo.com/ee/en/api/v4/upsellAggregation/parts/" + lastPath);
    //    httpRequest.Method = "POST";

    //    httpRequest.ContentType = "application/json";

    //    //var content = @"{""serialId"":"""",
    //    //             ""model"":"""",
    //    //             ""mtId"":""20ue""
    //    //            }";

    //    var content = @"{""serialId"":""" + split.ElementAtOrDefault(2) + @""",
    //                          ""model"":""" + split.ElementAtOrDefault(1) + @""",
    //                          ""mtId"":""" + split.ElementAtOrDefault(0) + @"""}";

    //    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream())) {
    //        streamWriter.Write(content);
    //    }
    //    try {
    //        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
    //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
    //            var result = streamReader.ReadToEnd();
    //            return result;
    //        }
    //    } catch (Exception ex) {
    //        System.Diagnostics.Debug.WriteLine($@"ERROR while fetching session storage from Lenovo :: {ex.Message} &-& {ex.InnerException}");
    //    }

    //    return null;
    //}

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
            //response = await client.GetAsync(client.BaseAddress);
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

            //newProduct.IsModelPart = false;
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

    /*private static async Task GetHtmlAsync(string url)
    {
        var html = await httpClient.GetStringAsync(url);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var nodes = htmlDocument.DocumentNode.QuerySelectorAll(".part-column");
        System.Diagnostics.Debug.WriteLine($"::::: {htmlDocument.DocumentNode.InnerHtml} :::::");

        foreach (HtmlNode node in nodes) {
            System.Diagnostics.Debug.WriteLine($":: {node.OuterHtml}");
        }
    }

    private static async Task<string> CallUrl(string fullUrl)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetStringAsync(fullUrl);
        return response;
    }

    private static List<string> ParseHtml(string html)
    {
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var programmerLinks = htmlDoc.DocumentNode.SelectNodes("//div[@class='part-column first']");

        List<string> wikiLink = new List<string>();

        if (programmerLinks is not null) {
            foreach (var link in programmerLinks) {
                if (link.FirstChild.Attributes.Count > 0) wikiLink.Add(link.FirstChild.Attributes[0].Value);
            }
        } else {
            System.Diagnostics.Debug.WriteLine($@":: no nodes found");
        }


        return wikiLink;
    }*/

    
}


