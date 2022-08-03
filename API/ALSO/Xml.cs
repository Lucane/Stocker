using System.Xml.Linq;

namespace Stocker.API.ALSO;

public class Xml
{
    public void ParseXml()
    {

    }

    public void PopulateDatabase()
    {

    }

    static public async Task GetXmlRequest()
    {
        XDocument xml = XDocument.Parse(@"<?xml version=""1.0"" encoding=""UTF - 8""?><CatalogRequest>" +
            @"<Route><From><ClientID>***REMOVED***</ClientID></From><To><ClientID>0</ClientID></To></Route>" +
            @"<Filters><Filter FilterID=""VendorID"" Value=""80008028"" /><Filter FilterID=""StockLevel"" Value=""Transit"" />" +
            @"<Filter FilterID=""Price"" Value=""WOVAT"" />" +
            @"</Filters></CatalogRequest>");


        var url = Uri.EscapeUriString(
            $@"https://b2b.also.ee/DirectXML.svc/0/scripts/XML_Interface.dll?USERNAME=***REMOVED***&PASSWORD=***REMOVED***&XML={xml}");

        var client = new HttpClient();
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode) {
            var dataObjects = response.Content.ReadAsStringAsync().Result;
            await File.WriteAllTextAsync(@$"C:\Users\Avalerion\Desktop\ALSO_data.xml", dataObjects);
        } else {
            await File.WriteAllTextAsync(@"C:\Users\Avalerion\Desktop\ALSO_data.log", $"{(int)response.StatusCode} ({response.ReasonPhrase})");
        }
    }

    static public async Task ParseApiResponse(string response)
    {

    }
}
