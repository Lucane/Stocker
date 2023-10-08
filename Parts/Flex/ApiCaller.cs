using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Stocker.Parts.Flex;


public class ApiCaller
{
    /// <summary>
    /// Sends a request to the Flex sales portal to retrieve the prices and stock availability for the provided part numbers.
    /// </summary>
    /// <param name="partNumbers">List of part number SKUs.</param>
    /// <returns>An object that contains the sanitized and deserialized JSON data.</returns>
    public static async Task<List<StockObject>?> GetStockStatus(List<string> partNumbers)
    {
        var response_auth = await RequestAuthTokens();
        ApiRemotingImplObject? object_auth = await ParseAuthTokens(response_auth);

        var response_stocks = await RequestStocks(partNumbers, object_auth);
        var response_prices = await RequestPrices(partNumbers, object_auth);

        if (response_stocks is null || response_prices is null) return null;

        return await DeserializeAndCombineResults(response_stocks, response_prices);

        //var populateResponse = await PopulateDatabase(apiResponse);
        //if (populateResponse is false) return null;

        //return await Task.FromResult(true);
    }

    /// <summary>
    /// Deserializes and combines the two separate plaintext JSON datasets.
    /// </summary>
    /// <param name="stockContent"></param>
    /// <param name="priceContent"></param>
    /// <returns>An object that contains the deserialized JSON data.</returns>
    public static async Task<List<StockObject>?> DeserializeAndCombineResults(string stockContent, string priceContent)
    {
        if (string.IsNullOrWhiteSpace(stockContent)) return null;

        ApiStockObject? object_stocks = null;
        ApiPriceObject? object_prices = null;

        try {
            object_stocks = JsonSerializer.Deserialize<List<ApiStockObject>>(stockContent).FirstOrDefault();
            object_prices = JsonSerializer.Deserialize<List<ApiPriceObject>>(priceContent).FirstOrDefault();

        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while deserializing response from Flex :: {ex.Message}{Environment.NewLine}{ex.InnerException}");
        }

        if (object_stocks is null || object_prices is null) return null;


        var allProducts = new List<StockObject>();

        foreach (var product in object_stocks.result.data.InvWarehouse) {
            var partNo = product.CC_Product__r.ccrz__SKU__c;
            var price = object_prices.result.data.Where(x => x.prodBean.SKU == partNo).Select(x => x.price).FirstOrDefault();
            price = Decimal.Round(price * (decimal)0.9, 2);      // As a Lenovo reseller & partner, we get a 10% discount.

            DateTime date = new DateTime();

            if (product.Venlo_Availability_Message__c.Contains('-')) {
                DateTime.TryParse(product.Venlo_Availability_Message__c, out date);
            }

            allProducts.Add(new StockObject {
                Warehouse = "Flex (Holland)",
                PartNumber = partNo,
                StockAvailable = product.Venlo_Stock__c,
                PriceDiscounted = price,
                DateIncoming = date
            });

            date = new DateTime();

            if (product.UK_Availability_Message__c.Contains('-')) {
                DateTime.TryParse(product.UK_Availability_Message__c, out date);
            }

            allProducts.Add(new StockObject {
                Warehouse = "Flex (UK)",
                PartNumber = partNo,
                StockAvailable = product.UK_Stock__c,
                PriceDiscounted = price,
                DateIncoming = date
            });

            date = new DateTime();

            if (product.Lenovo_Availability_Message__c.Contains('-')) {
                DateTime.TryParse(product.Lenovo_Availability_Message__c, out date);
            }

            allProducts.Add(new StockObject {
                Warehouse = "Flex (Hiina)",
                PartNumber = partNo,
                StockAvailable = product.Lenovo_Stock__c,
                PriceDiscounted = price,
                DateIncoming = date
            });
        }

        return allProducts;
    }

    /// <summary>
    /// Sends a POST request to fetch the JSON data containing the stock availability for the provided part numbers.
    /// </summary>
    /// <param name="partNumbers"></param>
    /// <param name="authTokens"></param>
    /// <returns>The web response content payload if the request was successful.</returns>
    public static async Task<string?> RequestStocks(List<string> partNumbers, ApiRemotingImplObject authTokens)
    {
        var vid = authTokens.vf.vid;
        var tokenIe = authTokens.actions.productListShowProduct.ms.Where(ms => ms.name == "InventoryBy_Warehouse").First();
        var ns = tokenIe.ns;
        var ver = tokenIe.ver;
        var csrfToken = tokenIe.csrf;
        var authToken = tokenIe.authorization;

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("referer", @"https://www.lenovopartsales.com/LenovoEsales/ccrz__HomePage");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://www.lenovopartsales.com/LenovoEsales/apexremote");
        request.Content = new StringContent($@"{{ ""action"": ""cc_lenovo_ctrl_ProductListShowProduct"",
                                                ""method"": ""InventoryBy_Warehouse"",
                                                ""type"": ""rpc"",
                                                ""tid"": 13,
                                                ""data"": [{{}},
                                                    {JsonSerializer.Serialize(partNumbers)}
                                                ],
                                                ""ctx"": {{
                                                    ""authorization"": ""{authToken}"",
                                                    ""csrf"": ""{csrfToken}"",
                                                    ""vid"": ""{vid}"",
                                                    ""ns"": ""{ns}"",
                                                    ""ver"": {ver}
                                                }},
                                                
                                            }}",
                                            Encoding.UTF8,
                                            "application/json");

        try {
            var response = await client.SendAsync(request);
            //response = await client.GetAsync(client.BaseAddress);
            System.Diagnostics.Debug.WriteLine($@"Response from Flex stock query :: {response}");
            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while performing Flex stock query :: {ex.Message}{Environment.NewLine}{ex.InnerException}");
        }

        return null;
    }

    /// <summary>
    /// Sends a POST request to fetch the JSON data containing the prices for the provided part numbers.
    /// </summary>
    /// <param name="partNumbers"></param>
    /// <param name="authTokens"></param>
    /// <returns>The web response content payload if the request was successful.</returns>
    public static async Task<string?> RequestPrices(List<string> partNumbers, ApiRemotingImplObject authTokens)
    {
        var serializedSKUs = "";

        foreach (var p in partNumbers) {
            serializedSKUs += @"{""sku"": """ + p + @"""},";
        }
        serializedSKUs = serializedSKUs.TrimEnd(',');      // Removes the extra comma that was added in the foreach loop.

        var vid = authTokens.vf.vid;
        var tokenIe = authTokens.actions.productListRd.ms.Where(ms => ms.name == "fetchPage").First();
        var ns = tokenIe.ns;
        var ver = tokenIe.ver;
        var csrfToken = tokenIe.csrf;
        var authToken = tokenIe.authorization;

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("referer", @"https://www.lenovopartsales.com/LenovoEsales/ccrz__HomePage");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://www.lenovopartsales.com/LenovoEsales/apexremote");
        request.Content = new StringContent($@"{{ ""action"": ""ccrz.cc_ctrl_ProductListRD"",
                                                ""method"": ""fetchPage"",
                                                ""type"": ""rpc"",
                                                ""tid"": 10,
                                                ""data"": [{{}}, [{serializedSKUs}]],
                                                ""ctx"": {{
                                                    ""authorization"": ""{authToken}"",
                                                    ""csrf"": ""{csrfToken}"",
                                                    ""vid"": ""{vid}"",
                                                    ""ns"": ""{ns}"",
                                                    ""ver"": {ver}
                                                }}
                                            }}",
                                            Encoding.UTF8,
                                            "application/json");

        try {
            var response = await client.SendAsync(request);
            System.Diagnostics.Debug.WriteLine($@"Response from Flex price query :: {response}");
            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while performing Flex price query :: {ex.Message}{Environment.NewLine}{ex.InnerException}");
        }

        return null;
    }

    /// <summary>
    /// Sends a GET request to fetch the CSRF and auth tokens for the Flex sales portal.
    /// </summary>
    /// <returns>The web response content payload if the request was successful.</returns>
    /// <remarks>Since the API endpoint is protected, tokens are required to send POST requests.</remarks>
    public static async Task<string?> RequestAuthTokens()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("referer", @"https://www.lenovopartsales.com/LenovoEsales/ccrz__HomePage");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://www.lenovopartsales.com/LenovoEsales/ccrz__Products");

        try {
            var response = await client.SendAsync(request);
            System.Diagnostics.Debug.WriteLine($@"Response from Flex price query :: {response}");
            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while performing Flex price query :: {ex.Message}{Environment.NewLine}{ex.InnerException}");
        }

        return null;
    }

    /// <summary>
    /// Sanitizes the returned JSON data with RegEx and then deserializes/objectifies the data.
    /// </summary>
    /// <param name="authContent"></param>
    /// <returns>An object that contains the sanitized and deserialized JSON data.</returns>
    public static async Task<ApiRemotingImplObject?> ParseAuthTokens(string authContent)
    {
        var authJson = Regex.Match(authContent, @"VFRM\.RemotingProviderImpl\(((.|\n)*?)\)\);").Groups[1].Value;

        ApiRemotingImplObject? object_auth = null;

        try {
            object_auth = JsonSerializer.Deserialize<ApiRemotingImplObject>(authJson);

        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while deserializing response from Flex :: {ex.Message}{Environment.NewLine}{ex.InnerException}");
            return null;
        }

        return object_auth;
    }
}