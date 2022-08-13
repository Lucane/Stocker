//
// https://json2csharp.com/
// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
//

using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace Stocker.API.ACC
{

    public class Rootobject
    {
        public string odatacontext { get; set; }
        public Product[] Products { get; set; }
    }

    public class Product
    {
        public string PID { get; set; }
        public string MPN { get; set; }
        public string EAN { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public Producer Producer { get; set; }
        public Price Price { get; set; }
        public Stock[] Stocks { get; set; }
        public bool Rent { get; set; }
        public bool ByOrder { get; set; }
        public bool ByOrderOrig { get; set; }
        public bool IsNew { get; set; }
        public bool IsDefect { get; set; }
        public string Bundle { get; set; }
        public bool EOLSale { get; set; }
        public bool HasCampaign { get; set; }
        public bool HasSmartPoint { get; set; }
        public bool IsEsd { get; set; }
        public decimal LatgaValue { get; set; }
        public decimal LatgaValueType { get; set; }
        public Campaign[] Campaigns { get; set; }
        public Saleout[] Saleouts { get; set; }
        public Branch[] Branches { get; set; }
        public Reserve Reserve { get; set; }
        public bool VisibleInB2B { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Warranty { get; set; }
        public bool HasBid { get; set; }
        public bool HasAttributes { get; set; }
        public Pricesforqty[] PricesForQty { get; set; }
        public string IntraCode { get; set; }
        public string CountryOfOrigin { get; set; }
        public object[] ProductDimensions { get; set; }
        public decimal QuantityPacking { get; set; }
        public object[] RRP { get; set; }
        public bool FullPackageShipping { get; set; }
    }

    public class Producer
    {
        public string OId { get; set; }
        public string Name { get; set; }
    }

    public class Price
    {
        public decimal Value { get; set; }
        public decimal OldValue { get; set; }
        public decimal LatgaValue { get; set; }
        public decimal LatgaOldValue { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? SmartPoints { get; set; }
        public int? SpCampaignId { get; set; }
        public bool IsSaleout { get; set; }
    }

    public class Reserve
    {
        public decimal OrderQty { get; set; }
        public decimal ReserveQty { get; set; }
        public decimal Price { get; set; }
        public decimal DedicatedReserveQty { get; set; }
        public string WhId { get; set; }
    }

    public class Stock
    {
        public string WhId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public decimal AmountArriving { get; set; }
        public decimal AmountOrdered { get; set; }
        public decimal AmountOrderedArrivingDiff { get; set; }
        public bool IsPreliminary { get; set; }
    }

    public class Campaign
    {
        public string OId { get; set; }
        public string Name { get; set; }
        public bool HasSmartPoint { get; set; }
    }

    public class Saleout
    {
        public int OId { get; set; }
    }

    public class Branch
    {
        public int OId { get; set; }
        public string Name { get; set; }
    }

    public class Pricesforqty
    {
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public string Currency { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
