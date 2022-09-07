//using K4os.Compression.LZ4;
using LZ4;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using StockerDB.Data.Stocker;
using Microsoft.EntityFrameworkCore;

namespace Stocker.Parts.Lenovo;

public class SessionStorage
{
    private readonly StockerContext _context;
    public SessionStorage(StockerContext context)
    {
        _context = context;
    }

    public async Task<bool> UpdateDevices()
    {
        // [REVIEW] reading the JSON from a file is only for testing purposes
        //var content = await FetchStorage();
        var content = await File.ReadAllTextAsync(@"C:\Users\Avalerion\Desktop\LZ4\Mse.AllProducts.us.en_RAW.txt");
        if (content is null) return false;

        CompressedObject compressedJson;
        string? decompressedContent;
        //System.Diagnostics.Debug.WriteLine($@"Content from Lenovo PCSupport :: {content}");

        try {
            compressedJson = JsonSerializer.Deserialize<CompressedObject>(content);
            var fromBase64 = Convert.FromBase64String(compressedJson.content);
            decompressedContent = await DecompressLZ4(fromBase64, compressedJson.originLength);
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while deserializing compressed JSON object :: {ex.Message} &-& {ex.InnerException}");
            return false;
        }

        List<ProductObject> decompressedJson;

        try {
            decompressedJson = JsonSerializer.Deserialize<List<ProductObject>>(decompressedContent);
            if (decompressedJson is null) return false;
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while deserializing Lenovo devices JSON :: {ex.Message} &-& {ex.InnerException}");
            return false;
        }

        var allProducts = new List<LenovoDevices>();

        await _context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.LenovoDevices");
        var updatedAt = DateTime.Now;

        foreach (var device in decompressedJson) {
            if (device.Type != "Product.SubSeries") continue;
            var newProduct = new LenovoDevices();

            newProduct.FullGUID = device.FullGuid;
            newProduct.ImageURL = device.Image;
            newProduct.LastUpdated = updatedAt;
            newProduct.Name = device.Name;
            newProduct.ParentGUID = device.ParentID;
            newProduct.ProductGUID = device.ProductGuid;
            newProduct.ProductID = device.Id;
            newProduct.ProductURL = @"https://pcsupport.lenovo.com/ee/en/products/" + newProduct.ProductID + @"/parts/display/compatible";

            _context.Add(newProduct);
        }

        _context.SaveChanges();

        return true;
    }

    public async Task<string?> DecompressLZ4(byte[] content, int originLength)
    {
        var decoded = LZ4Codec.Decode(content, 0, content.Length, originLength);
        var str = Encoding.UTF8.GetString(decoded);
        return str;
    }

    public async Task<string?> FetchStorage()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://pcsupport.lenovo.com/ee/en/api/v4/mse/getAllProducts?productId=");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Referrer = new Uri("https://pcsupport.lenovo.com/ee/en/");
        client.DefaultRequestHeaders.Add("X-CSRF-Token", "2yukcKMb1CvgPuIK9t04C6");
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
        client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

        try {
            var response = await client.GetAsync(client.BaseAddress);
            System.Diagnostics.Debug.WriteLine($@"Response from Lenovo PCSupport :: {response}");
            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while fetching session storage from Lenovo :: {ex.Message} &-& {ex.InnerException}");
        }

        return null;
    }
}
