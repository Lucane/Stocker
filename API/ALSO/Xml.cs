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

    static public async Task<XDocument?> GetXmlRequest()
    {
        XDocument query = XDocument.Parse(@"<?xml version=""1.0"" encoding=""UTF - 8""?><CatalogRequest>" +
            @"<Route><From><ClientID>***REMOVED***</ClientID></From><To><ClientID>0</ClientID></To></Route>" +
            @"<Filters><Filter FilterID=""VendorID"" Value=""80008028"" /><Filter FilterID=""StockLevel"" Value=""Transit"" />" +
            @"<Filter FilterID=""Price"" Value=""WOVAT"" />" +
            @"</Filters></CatalogRequest>");


        var url = Uri.EscapeUriString(
            $@"https://b2b.also.ee/DirectXML.svc/0/scripts/XML_Interface.dll?USERNAME=***REMOVED***&PASSWORD=***REMOVED***&XML={query}");

        var client = new HttpClient();
        var response = await client.GetAsync(url);

        System.Diagnostics.Debug.WriteLine($@"Response from ALSO API :: ({response.StatusCode}) - {response.ReasonPhrase}");
        //System.Diagnostics.Debug.WriteLine(XDocument.Parse(await response.Content.ReadAsStringAsync()));

        if (response.IsSuccessStatusCode) {
            return XDocument.Parse(await response.Content.ReadAsStringAsync());
            //var formattedXml = FormatXml(response.Content.ReadAsStringAsync().Result);
            
        } else {
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
