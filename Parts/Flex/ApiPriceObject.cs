using System.Text.Json.Serialization;

namespace Stocker.Parts.Flex;


// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);

public class ApiPriceObject
{
    public int statusCode { get; set; }
    public string type { get; set; }
    public int tid { get; set; }
    public bool @ref { get; set; }
    public string action { get; set; }
    public string method { get; set; }
    public PriceResult result { get; set; }
}

public class Datum
{
    public decimal price { get; set; }
    public decimal basePrice { get; set; }
    public decimal savings { get; set; }
    public bool showNewSubscriptionSelection { get; set; }
    public double avgRating { get; set; }
    public ProdBean prodBean { get; set; }
    public int inventory { get; set; }
    public bool showPricing { get; set; }
    public int qtySingleIncrement { get; set; }
    public bool canAddtoCart { get; set; }
}

public class PriceInputContext
{
}

public class ProdBean
{
    public string sfid { get; set; }
    public string sfdcName { get; set; }
    public string ownerId { get; set; }
    public string SKU { get; set; }
    public string storefront { get; set; }
    public object startDate { get; set; }
    public object endDate { get; set; }
    public double averageRating { get; set; }
    public string SEODescription { get; set; }
    public string SEOTitle { get; set; }
    public bool taxable { get; set; }
    public string inventoryType { get; set; }
    public bool coolerpakFlag { get; set; }
    public bool dryiceFlag { get; set; }
    public bool groundFlag { get; set; }
    public bool airFlag { get; set; }
    public bool hazardousFlag { get; set; }
    public bool hazardousOutside48Flag { get; set; }
    public int leadTime { get; set; }
    public bool overnightFlag { get; set; }
    public string productId { get; set; }
    public bool reviewFlag { get; set; }
    public bool serviceFlag { get; set; }
    public bool shipSeparately { get; set; }
    public bool shippedIndividually { get; set; }
    public int numberOfReviews { get; set; }
    public double totalRating { get; set; }
    public string sfdcCurrencyISOCode { get; set; }
    public List<ProductInventoryItemsS> productInventoryItemsS { get; set; }
    public string shortDesc { get; set; }
    public string longDesc { get; set; }
    public decimal price { get; set; }
    public bool showNewSubscriptionSelection { get; set; }
    public string id { get; set; }
    public string sku { get; set; }
    public string ProductType { get; set; }
    public string ProductStatus { get; set; }
    public string availMsg { get; set; }
    public double qtyPerUnit { get; set; }
    public string UnitOfMeasure { get; set; }
    public bool showSubscriptionSelection { get; set; }
    public string name { get; set; }
}

public class ProductInventoryItemsS
{
    public string productItem { get; set; }
    public string sfid { get; set; }
    public string sfdcName { get; set; }
    public int qtyAvailable { get; set; }
    public string sfdcCurrencyISOCode { get; set; }
}

public class PriceResult
{
    public List<Datum> data { get; set; }
    public PriceInputContext inputContext { get; set; }
    public List<object> messages { get; set; }
    public bool success { get; set; }
}



/*public class ApiPriceObject
{
    [JsonPropertyName("Class1")]
    public PriceRoot[] RootArray { get; set; }
}

public class PriceRoot
{
    public int statusCode { get; set; }
    public string type { get; set; }
    public int tid { get; set; }
    [JsonPropertyName("ref")]
    public bool _ref { get; set; }
    public string action { get; set; }
    public string method { get; set; }
    public PriceResult result { get; set; }
}

public class PriceResult
{
    public Datum[] data { get; set; }
    public PriceInputcontext inputContext { get; set; }
    public object[] messages { get; set; }
    public bool success { get; set; }
}

public class PriceInputcontext
{
}

public class Datum
{
    public decimal price { get; set; }
    public int basePrice { get; set; }
    public int savings { get; set; }
    public bool showNewSubscriptionSelection { get; set; }
    public float avgRating { get; set; }
    public Prodbean prodBean { get; set; }
    public int inventory { get; set; }
    public bool showPricing { get; set; }
    public int qtySingleIncrement { get; set; }
    public bool canAddtoCart { get; set; }
}

public class Prodbean
{
    public string sfid { get; set; }
    public string sfdcName { get; set; }
    public string ownerId { get; set; }
    public string SKU { get; set; }
    public string storefront { get; set; }
    public long startDate { get; set; }
    public long endDate { get; set; }
    public float averageRating { get; set; }
    public string SEODescription { get; set; }
    public string SEOTitle { get; set; }
    public bool taxable { get; set; }
    public string inventoryType { get; set; }
    public bool coolerpakFlag { get; set; }
    public bool dryiceFlag { get; set; }
    public bool groundFlag { get; set; }
    public bool airFlag { get; set; }
    public bool hazardousFlag { get; set; }
    public bool hazardousOutside48Flag { get; set; }
    public int leadTime { get; set; }
    public bool overnightFlag { get; set; }
    public string productId { get; set; }
    public bool reviewFlag { get; set; }
    public bool serviceFlag { get; set; }
    public bool shipSeparately { get; set; }
    public bool shippedIndividually { get; set; }
    public int numberOfReviews { get; set; }
    public float totalRating { get; set; }
    public string sfdcCurrencyISOCode { get; set; }
    public Productinventoryitemss[] productInventoryItemsS { get; set; }
    public string shortDesc { get; set; }
    public string longDesc { get; set; }
    public decimal price { get; set; }
    public bool showNewSubscriptionSelection { get; set; }
    public string id { get; set; }
    public string sku { get; set; }
    public string ProductType { get; set; }
    public string ProductStatus { get; set; }
    public string availMsg { get; set; }
    public float qtyPerUnit { get; set; }
    public string UnitOfMeasure { get; set; }
    public bool showSubscriptionSelection { get; set; }
    public string name { get; set; }
}

public class Productinventoryitemss
{
    public string productItem { get; set; }
    public string sfid { get; set; }
    public string sfdcName { get; set; }
    public int qtyAvailable { get; set; }
    public string sfdcCurrencyISOCode { get; set; }
}*/