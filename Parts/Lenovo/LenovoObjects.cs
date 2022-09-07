namespace Stocker.Parts.Lenovo;


public class ParsedObject
{
    public bool IsModelPart { get; set; }
    public string PartNumber { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public List<string>? CompatibleModels { get; set; }
    public List<string>? ImageURLs { get; set; }
    public List<string>? Substitutes { get; set; }
    public List<string>? ExternalIDs { get; set; }
}

public class Root
{
    public int code { get; set; }
    public int costMillis { get; set; }
    public string requestId { get; set; }
    public string responseHost { get; set; }
    public Msg msg { get; set; }
    public Datum[] data { get; set; }
}

public class Msg
{
    public string desc { get; set; }
    public object value { get; set; }
}

public class Datum
{
    public string id { get; set; }
    public string name { get; set; }
    public string source { get; set; }
    public string bacCode { get; set; }
    public string[] imageNames { get; set; }
    public string[] imageUrls { get; set; }
    public DateTime? imagesUpdated { get; set; }
    public string commodityVal { get; set; }
    public Commodity commodity { get; set; }
    public string[] products { get; set; }
    public Substitute[] substitutes { get; set; }
    public string cruTier { get; set; }
    public Salability salability { get; set; }
    public string[] externalIds { get; set; }
    public string[] bundles { get; set; }
    public bool motionless { get; set; }
    public bool screen { get; set; }
    public object redemption { get; set; }
    public string parentId { get; set; }
    public object[] supplierInfos { get; set; }
    public Localization[] localizations { get; set; }
    public string externalId { get; set; }
    public string barcode { get; set; }
    public int quantity { get; set; }
    public Quantity[] quantities { get; set; }
}

public class Commodity
{
    public string source { get; set; }
    public string type { get; set; }
    public string id { get; set; }
    public string language { get; set; }
    public string value { get; set; }
}

public class Salability
{
    public string id { get; set; }
    public bool isCanSalability { get; set; }
    public int stock { get; set; }
    public int realStock { get; set; }
    public int oversellStock { get; set; }
    public bool isAllowedOversold { get; set; }
    public float value { get; set; }
    public object formattedValue { get; set; }
    public float tax { get; set; }
    public float discount { get; set; }
    public float includingTaxValue { get; set; }
    public object formattedIncludingTaxValue { get; set; }
    public float includingDiscountValue { get; set; }
    public object formattedIncludingDiscountValue { get; set; }
    public float includingTaxAndDiscountValue { get; set; }
    public object formattedIncludingTaxAndDiscountValue { get; set; }
    public object rmaValue { get; set; }
    public object includingDiscountRmaValue { get; set; }
    public object formattedRMAValue { get; set; }
    public object formattedIncludingDiscountRMAValue { get; set; }
    public object buyUrl { get; set; }
    public object currency { get; set; }
    public object culture { get; set; }
    public object symbol { get; set; }
    public Taxrate taxRate { get; set; }
    public object priceStartDate { get; set; }
    public object priceEndDate { get; set; }
    public bool saleStatus { get; set; }
    public object[] dcgCostPrice { get; set; }
}

public class Taxrate
{
    public object materialType { get; set; }
    public object strItemCategoryGroup { get; set; }
    public object linkingHierarchyCode { get; set; }
    public object unspsCode { get; set; }
    public object netWeight { get; set; }
}

public class Substitute
{
    public string type { get; set; }
    public string id { get; set; }
    public object[] products { get; set; }
}

public class Localization
{
    public string language { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string loisDescription { get; set; }
}

public class Quantity
{
    public string partFruID { get; set; }
    public int quantity { get; set; }
    public string partPPNID { get; set; }
}