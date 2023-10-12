namespace Stocker.API.F9;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class PriceCatalog
{

    private PriceCatalogPriceCatHdr priceCatHdrField;

    private PriceCatalogCatalogItem[] listofCatalogDetailsField;

    /// <remarks/>
    public PriceCatalogPriceCatHdr PriceCatHdr
    {
        get
        {
            return this.priceCatHdrField;
        }
        set
        {
            this.priceCatHdrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("CatalogItem", IsNullable = false)]
    public PriceCatalogCatalogItem[] ListofCatalogDetails
    {
        get
        {
            return this.listofCatalogDetailsField;
        }
        set
        {
            this.listofCatalogDetailsField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogPriceCatHdr
{

    private System.DateTime dateField;

    private string currencyField;

    private string descriptionField;

    private object catNumberField;

    private PriceCatalogPriceCatHdrFilters filtersField;

    private PriceCatalogPriceCatHdrRoute routeField;

    /// <remarks/>
    public System.DateTime Date
    {
        get
        {
            return this.dateField;
        }
        set
        {
            this.dateField = value;
        }
    }

    /// <remarks/>
    public string Currency
    {
        get
        {
            return this.currencyField;
        }
        set
        {
            this.currencyField = value;
        }
    }

    /// <remarks/>
    public string Description
    {
        get
        {
            return this.descriptionField;
        }
        set
        {
            this.descriptionField = value;
        }
    }

    /// <remarks/>
    public object CatNumber
    {
        get
        {
            return this.catNumberField;
        }
        set
        {
            this.catNumberField = value;
        }
    }

    /// <remarks/>
    public PriceCatalogPriceCatHdrFilters Filters
    {
        get
        {
            return this.filtersField;
        }
        set
        {
            this.filtersField = value;
        }
    }

    /// <remarks/>
    public PriceCatalogPriceCatHdrRoute Route
    {
        get
        {
            return this.routeField;
        }
        set
        {
            this.routeField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogPriceCatHdrFilters
{

    private PriceCatalogPriceCatHdrFiltersFilter filterField;

    /// <remarks/>
    public PriceCatalogPriceCatHdrFiltersFilter Filter
    {
        get
        {
            return this.filterField;
        }
        set
        {
            this.filterField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogPriceCatHdrFiltersFilter
{

    private string filterIDField;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FilterID
    {
        get
        {
            return this.filterIDField;
        }
        set
        {
            this.filterIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogPriceCatHdrRoute
{

    private PriceCatalogPriceCatHdrRouteFrom fromField;

    private PriceCatalogPriceCatHdrRouteTO toField;

    /// <remarks/>
    public PriceCatalogPriceCatHdrRouteFrom From
    {
        get
        {
            return this.fromField;
        }
        set
        {
            this.fromField = value;
        }
    }

    /// <remarks/>
    public PriceCatalogPriceCatHdrRouteTO To
    {
        get
        {
            return this.toField;
        }
        set
        {
            this.toField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogPriceCatHdrRouteFrom
{

    private object clientIDField;

    /// <remarks/>
    public object ClientID
    {
        get
        {
            return this.clientIDField;
        }
        set
        {
            this.clientIDField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogPriceCatHdrRouteTO
{

    private ushort clientIDField;

    /// <remarks/>
    public ushort ClientID
    {
        get
        {
            return this.clientIDField;
        }
        set
        {
            this.clientIDField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogCatalogItem
{

    private PriceCatalogCatalogItemProduct productField;

    private PriceCatalogCatalogItemQty[] qtyField;

    private PriceCatalogCatalogItemPrice priceField;

    private PriceCatalogCatalogItemBundleItem[] bundleInfoField;

    /// <remarks/>
    public PriceCatalogCatalogItemProduct Product
    {
        get
        {
            return this.productField;
        }
        set
        {
            this.productField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Qty")]
    public PriceCatalogCatalogItemQty[] Qty
    {
        get
        {
            return this.qtyField;
        }
        set
        {
            this.qtyField = value;
        }
    }

    /// <remarks/>
    public PriceCatalogCatalogItemPrice Price
    {
        get
        {
            return this.priceField;
        }
        set
        {
            this.priceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("BundleItem", IsNullable = false)]
    public PriceCatalogCatalogItemBundleItem[] BundleInfo
    {
        get
        {
            return this.bundleInfoField;
        }
        set
        {
            this.bundleInfoField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogCatalogItemProduct
{

    private uint productIDField;

    private string partNumberField;

    private string eANCodeField;

    private string linkField;

    private string descriptionField;

    private string longDescField;

    private string periodofOnsiteWarrantyField;

    private string periodofWarrantyField;

    private PriceCatalogCatalogItemProductGroupBy[] groupingField;

    /// <remarks/>
    public uint ProductID
    {
        get
        {
            return this.productIDField;
        }
        set
        {
            this.productIDField = value;
        }
    }

    /// <remarks/>
    public string PartNumber
    {
        get
        {
            return this.partNumberField;
        }
        set
        {
            this.partNumberField = value;
        }
    }

    /// <remarks/>
    public string EANCode
    {
        get
        {
            return this.eANCodeField;
        }
        set
        {
            this.eANCodeField = value;
        }
    }

    /// <remarks/>
    public string Link
    {
        get
        {
            return this.linkField;
        }
        set
        {
            this.linkField = value;
        }
    }

    /// <remarks/>
    public string Description
    {
        get
        {
            return this.descriptionField;
        }
        set
        {
            this.descriptionField = value;
        }
    }

    /// <remarks/>
    public string LongDesc
    {
        get
        {
            return this.longDescField;
        }
        set
        {
            this.longDescField = value;
        }
    }

    /// <remarks/>
    public string PeriodofOnsiteWarranty
    {
        get
        {
            return this.periodofOnsiteWarrantyField;
        }
        set
        {
            this.periodofOnsiteWarrantyField = value;
        }
    }

    /// <remarks/>
    public string PeriodofWarranty
    {
        get
        {
            return this.periodofWarrantyField;
        }
        set
        {
            this.periodofWarrantyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("GroupBy", IsNullable = false)]
    public PriceCatalogCatalogItemProductGroupBy[] Grouping
    {
        get
        {
            return this.groupingField;
        }
        set
        {
            this.groupingField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogCatalogItemProductGroupBy
{

    private string groupIDField;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string GroupID
    {
        get
        {
            return this.groupIDField;
        }
        set
        {
            this.groupIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogCatalogItemQty
{

    private ushort qtyAvailableField;

    private System.DateTime deliveryDateField;

    private bool deliveryDateFieldSpecified;

    private byte warehouseIDField;

    /// <remarks/>
    public ushort QtyAvailable
    {
        get
        {
            return this.qtyAvailableField;
        }
        set
        {
            this.qtyAvailableField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
    public System.DateTime DeliveryDate
    {
        get
        {
            return this.deliveryDateField;
        }
        set
        {
            this.deliveryDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DeliveryDateSpecified
    {
        get
        {
            return this.deliveryDateFieldSpecified;
        }
        set
        {
            this.deliveryDateFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte WarehouseID
    {
        get
        {
            return this.warehouseIDField;
        }
        set
        {
            this.warehouseIDField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogCatalogItemPrice
{

    private PriceCatalogCatalogItemPriceUnitPrice[] unitPriceField;

    private PriceCatalogCatalogItemPriceQtyDiscount qtyDiscountField;

    private byte promoFlagField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("UnitPrice")]
    public PriceCatalogCatalogItemPriceUnitPrice[] UnitPrice
    {
        get
        {
            return this.unitPriceField;
        }
        set
        {
            this.unitPriceField = value;
        }
    }

    /// <remarks/>
    public PriceCatalogCatalogItemPriceQtyDiscount QtyDiscount
    {
        get
        {
            return this.qtyDiscountField;
        }
        set
        {
            this.qtyDiscountField = value;
        }
    }

    /// <remarks/>
    public byte PromoFlag
    {
        get
        {
            return this.promoFlagField;
        }
        set
        {
            this.promoFlagField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogCatalogItemPriceUnitPrice
{

    private string typeField;

    private decimal valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public decimal Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogCatalogItemPriceQtyDiscount
{

    private byte minQtyField;

    private decimal discPriceField;

    /// <remarks/>
    public byte MinQty
    {
        get
        {
            return this.minQtyField;
        }
        set
        {
            this.minQtyField = value;
        }
    }

    /// <remarks/>
    public decimal DiscPrice
    {
        get
        {
            return this.discPriceField;
        }
        set
        {
            this.discPriceField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PriceCatalogCatalogItemBundleItem
{

    private string partNumberField;

    private string eANCodeField;

    private byte qtyField;

    /// <remarks/>
    public string PartNumber
    {
        get
        {
            return this.partNumberField;
        }
        set
        {
            this.partNumberField = value;
        }
    }

    /// <remarks/>
    public string EANCode
    {
        get
        {
            return this.eANCodeField;
        }
        set
        {
            this.eANCodeField = value;
        }
    }

    /// <remarks/>
    public byte Qty
    {
        get
        {
            return this.qtyField;
        }
        set
        {
            this.qtyField = value;
        }
    }
}

