using System.Text;
using System.Text.Json;
using Stocker.Parts.Flex;

namespace Stocker.Parts.Flex;


public class ApiCaller
{
    public static async Task<List<StockObject>?> GetStockStatus(List<string> partNumbers)
    {
        var response_stocks = await RequestStocks(partNumbers);
        var response_prices = await RequestPrices(partNumbers);

        if (response_stocks is null || response_prices is null) return null;

        return await DeserializeAndCombineRequests(response_stocks, response_prices);

        //var populateResponse = await PopulateDatabase(apiResponse);
        //if (populateResponse is false) return null;

        //return await Task.FromResult(true);
    }

    public static async Task<List<StockObject>?> DeserializeAndCombineRequests(string stockContent, string priceContent)
    {
        if (string.IsNullOrWhiteSpace(stockContent)) return null;

        //stockContent = stockContent.Trim('[').Trim(']');
        //priceContent = priceContent.Trim('[').Trim(']');

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
            price = Decimal.Round(price * (decimal)0.9, 2);      // as a Lenovo reseller & partner, we get a 10% discount

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

    public static async Task<string?> RequestStocks(List<string> partNumbers)
    {
        /*{"data":[{
                                "storefront":"LenovoEsales",
              "userIsoCode":"EUR",
              "userLocale":"en_US",
              "currentPageName":"ccrz_Products"},
                     ["5CB0Z69532", "5CB0Z69533"]
                    ],
            "tid":13,
            "ctx":{
                                "csrf":"VmpFPSxNakF5TWkwd09DMHlNRlF3T0RveE16b3lNUzQxTmpoYSxDYzRKTFZTWVdOX24wZHZuWThUc3hnLE1HTXpNVE15",
                  "ns":"",
                  "ver":41,
                  "vid":"066a0000000CqD0"},
            "type":"rpc",
            "action":"cc_lenovo_ctrl_ProductListShowProduct",
            "method":"InventoryBy_Warehouse"
        }*/


        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("referer", @"https://www.lenovopartsales.com/LenovoEsales/ccrz__Products?cartID=&portalUser=&store=&cclcl=en_US&operation=quickSearch&searchText=t14");

        //var requestObj = new RequestContentObj();
        //requestObj.data.currentPageName = 
        //request.

        //requestObj.ctx.csrf = "VmpFPSxNakF5TWkwd09DMHlNRlF3T0RveE16b3lNUzQxTmpoYSxDYzRKTFZTWVdOX24wZHZuWThUc3hnLE1HTXpNVE15";
        //requestObj.ctx.ns = "";
        //requestObj.ctx.ver = 41;
        //requestObj.ctx.vid = "066a0000000CqD0";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://www.lenovopartsales.com/LenovoEsales/apexremote");
        request.Content = new StringContent(@"{""data"":[{
                                                      ""storefront"":""LenovoEsales"",
                                                      ""userIsoCode"":""EUR"",
                                                      ""userLocale"":""en_US"",
                                                      ""currentPageName"":""ccrz_Products""}," +
                                                         JsonSerializer.Serialize(partNumbers) +
                                                        @"],
                                                        ""tid"":11,
                                                        ""ctx"":{""csrf"":""VmpFPSxNakF5TWkwd09DMHlNVlF4T0RveU5EbzFPUzQ0TURaYSxDN283N0tvT0IxaW95N2ZJV1pPWXIwLE1HTXpNVE16"",
	                                                            ""ns"":"""",
	                                                            ""ver"":41,
	                                                            ""vid"":""066a0000000CqD0""},
                                                        ""type"":""rpc"",
                                                        ""action"":""cc_lenovo_ctrl_ProductListShowProduct"",
                                                        ""method"":""InventoryBy_Warehouse""}",
                                            Encoding.UTF8,
                                            "application/json");

        //System.Diagnostics.Debug.WriteLine($@"request content :: {await request.Content.ReadAsStringAsync()}");

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

    public static async Task<string?> RequestPrices(List<string> partNumbers)
    {
        var serializedSKUs = "";

        foreach (var p in partNumbers) {
            serializedSKUs += @"{""sku"": """ + p + @"""},";
        }

        serializedSKUs = serializedSKUs.TrimEnd(',');      // removes the extra comma that was added in the foreach loop

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("referer", @"https://www.lenovopartsales.com/LenovoEsales/ccrz__Products?cartID=&portalUser=&store=&cclcl=en_US&operation=quickSearch&searchText=t14");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://www.lenovopartsales.com/LenovoEsales/apexremote");
        request.Content = new StringContent(@"{ ""action"": ""ccrz.cc_ctrl_ProductListRD"",
                                                ""method"": ""fetchPage"",
                                                ""data"": [{},
                                                    ["
                                                        + serializedSKUs +
                                                  @"]
                                                ],
                                                ""type"": ""rpc"",
                                                ""tid"": 10,
                                                ""ctx"": {
                                                    ""csrf"": ""VmpFPSxNakF5TWkwd09DMHlNVlF5TURveU56bzBNeTQwTkRKYSxQSWtZa19nbGVqTGlNdHBoVGh6aTRnLE5HRTJNamcz"",
                                                    ""vid"": ""066a0000000CqD0"",
                                                    ""ns"": ""ccrz"",
                                                    ""ver"": 50
                                                }
                                            }",
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
}
