﻿using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace Stocker.API.Lenovo;

public class Product
{
    public class ProductObject
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string Image { get; set; }
        public DateTime Released { get; set; }
        public List<string> OperatingSystems { get; set; }
    }

    [Inject]
    private IConfiguration _config { get; set; }

    /// <summary>
    /// Returns the API response as a deserialized object of <see cref="ProductObject"/>.<br/>
    /// Returns <see langword="null"/> instead if the API request was unsuccessful or if the deserialization failed.
    /// </summary>
    /// <param name="productID">Product ID specified by either <c>ModelType</c>, Model or Serial.</param>
    /// <returns></returns>
    public async Task<ProductObject?> GetDetailsAsync(string productID)
    {
        var apiResponse = await SendRequestAsync(productID);
        if (apiResponse is null) return null;

        ProductObject? responseObject = null;

        try {
            responseObject = JsonSerializer.Deserialize<ProductObject>(apiResponse);

        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while deserializing Lenovo API response :: {ex.Message} &-& {ex.InnerException}");
        }

        return responseObject;
    }

    /// <summary>
    /// Performs an API POST request and returns the response content as <see cref="string"/>.<br/>
    /// Returns <see langword="null"/> instead if the request was unsuccessful.
    /// </summary>
    /// <param name="productID"></param>
    /// <returns></returns>
    private async Task<string?> SendRequestAsync(string productID)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("ClientID", _config["ApiKeys:Lenovo:clientId"]);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"https://supportapi.lenovo.com/v2.5/Product?ID={productID}");

        try {
            var response = await client.SendAsync(request);
            System.Diagnostics.Debug.WriteLine($@"Response from Lenovo API :: {request.RequestUri}
                                                                              {response}");
            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"[ERROR] while performing Lenovo API query :: {request.RequestUri}
                                                                                               {ex.Message}
                                                                                               {ex.InnerException}");
        }

        return null;
    }
}
