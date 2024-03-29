﻿namespace Stocker.Parts.Flex;

public class ApiStockObject
{
    public int statusCode { get; set; }
    public string type { get; set; }
    public int tid { get; set; }
    public bool @ref { get; set; }
    public string action { get; set; }
    public string method { get; set; }
    public StockResult result { get; set; }
}

public class CCProductR
{
    public string ccrz__SKU__c { get; set; }
    public string Id { get; set; }
    public string CurrencyIsoCode { get; set; }
}

public class Data
{
    public List<InvWarehouse> InvWarehouse { get; set; }
}

public class StockInputContext
{
    public string currentPageName { get; set; }
    public string storefront { get; set; }
    public string userIsoCode { get; set; }
    public string userLocale { get; set; }
}

public class InvWarehouse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string CC_Product__c { get; set; }
    public int UK_Stock__c { get; set; }
    public int Venlo_Stock__c { get; set; }
    public int Lenovo_Stock__c { get; set; }
    public string UK_Availability_Message__c { get; set; }
    public string Lenovo_Availability_Message__c { get; set; }
    public string Venlo_Availability_Message__c { get; set; }
    public string CurrencyIsoCode { get; set; }
    public CCProductR CC_Product__r { get; set; }
}

public class StockResult
{
    public Data data { get; set; }
    public StockInputContext inputContext { get; set; }
    public List<object> messages { get; set; }
    public bool success { get; set; }
}