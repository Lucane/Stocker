using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
namespace Stocker.API.ALSO;

public class Xml
{
    public class Globals
    {
        //public static Settings? SETTINGS = Formula_1_Media_Handler.Properties.Settings.Default;
        //public static readonly string? EXE_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly string EXE_PATH = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string LOG_PATH = $@"{EXE_PATH}\stocker.log";
        public static readonly string PRODUCTS_ALSO_XML_PATH = $@"{EXE_PATH}\XML\products_also.xml";
    }

    public void ParseXml()
    {

    }

    //static public async Task PopulateDatabase()
    //{
    //    var xDoc = XDocument.Parse(await GetXmlRequest() ?? "");
    //    if (String.IsNullOrWhiteSpace(xDoc.ToString())) return;

    //    var products = xDoc.Element("PriceCatalog")?.Element("ListofCatalogDetails")?.Elements("CatalogItem");
    //    if (products?.Count() == 0) return;


    //}

    public enum Warehouse
    {
        ALSO,
        ACC,
        F9
    }

    static public async Task<XDocument?> GetXmlRequest(Warehouse wh)
    {
        var client = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();
        string query;

        switch (wh) {
            case Warehouse.ALSO: { 
                    var request = XDocument.Parse(@"<?xml version=""1.0"" encoding=""UTF - 8""?><CatalogRequest>" +
                            @"<Route><From><ClientID>***REMOVED***</ClientID></From><To><ClientID>0</ClientID></To></Route>" +
                            @"<Filters><Filter FilterID=""VendorID"" Value=""80008028"" /><Filter FilterID=""StockLevel"" Value=""Transit"" />" +
                            @"<Filter FilterID=""Price"" Value=""WOVAT"" />" +
                            @"</Filters></CatalogRequest>");

                    query = Uri.EscapeUriString($@"https://b2b.also.ee/DirectXML.svc/0/scripts/XML_Interface.dll?USERNAME=***REMOVED***&PASSWORD=***REMOVED***&XML={request}");
                    response = await client.GetAsync(query);
                } break;

            case Warehouse.ACC: {
                    client.BaseAddress = new Uri("https://api.accdistribution.net/");
                    //client.DefaultRequestHeaders.Add("Host", "api.accdistribution.net");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    

                    //var request = @"{" + 
                    //    @"""request"": {" +
                    //        @"""LicenseKey"": ""***REMOVED***""," +
                    //        @""": ""EN""," +
                    //        @"""Currency"": ""EUR""," +
                    //        @"""CompanyId"": ""_al""," +
                    //        @"""Offset"": ""0""," +
                    //        @"""Limit"": ""1000""," +
                    //        //@"""Filters"": [" +
                    //        //@"{" +
                    //        //    @"""producer"": ""LV""," +
                    //        //    @"""stockFlag: [" +
                    //        //        @"""hasStock""," +
                    //        //        @"""arrivingStock""" +
                    //        //    @"]" +
                    //        //@"}]" +
                    //    @"}" +
                    //@"}";

                    // ,\"Filters\":{\"producer\":\"LV\"}
                    var request = new HttpRequestMessage(HttpMethod.Post, "v1/GetProducts");
                    request.Content = new StringContent("{\"request\":{\"LicenseKey\":\"***REMOVED***\",\"Locale\":\"en\",\"Currency\":\"EUR\",\"CompanyId\":\"_al\"}}",
                        Encoding.UTF8, "application/json");
                    

                    try{
                        //var requestString = new StringContent(request.Replace(Environment.NewLine, ""), Encoding.UTF8, "application/json");
                        //response = await client.SendAsync(, requestString);

                        await client.SendAsync(request)
                                .ContinueWith(responseTask => {
                                    System.Diagnostics.Debug.WriteLine($@"Response from {wh} API :: ({responseTask.Status})");
                                    System.Diagnostics.Debug.WriteLine("RESULT :: " + responseTask.Result);
                                    System.Diagnostics.Debug.WriteLine("CONTENT :: " + responseTask.Result.Content.ReadAsStringAsync().Result);
                                });

                        System.Diagnostics.Debug.WriteLine("finished processing API");
                        return null;
                    } catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine($@"&-& {ex.Message} &-& {ex.InnerException}");
                        return null;
                    }
                    
                } break;
        }

        if (response != null && response.IsSuccessStatusCode) {
            System.Diagnostics.Debug.WriteLine($@"Response from {wh} API :: ({response.StatusCode}) - {response.ReasonPhrase}");
            return XDocument.Parse(await response.Content.ReadAsStringAsync());
            //var formattedXml = FormatXml(response.Content.ReadAsStringAsync().Result);
        } else {
            System.Diagnostics.Debug.WriteLine($@"Response from {wh} API :: ({response.StatusCode}) - {response.ReasonPhrase}");
            return null;
            //await File.WriteAllTextAsync($@"{Globals.EXE_PATH}\XML\products_also.log", $"{(int)response.StatusCode} ({response.ReasonPhrase})");
        }
    }

    static public string FormatXml (string rawXml)
    {
        try {
            XDocument doc = XDocument.Parse(rawXml);
            return doc.ToString();
        } catch (Exception) {
            // Handle and throw if fatal exception here; don't just ignore them
            return rawXml;
        }
    }

    static public async Task ParseApiResponse(string response)
    {

    }
}
