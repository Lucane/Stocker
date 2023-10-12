using System.Xml.Serialization;

namespace Stocker.API.ALSO
{
    /// <remarks/>
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
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
        [XmlArrayItem("CatalogItem", IsNullable = false)]
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
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogPriceCatHdr
    {

        private System.DateTime dateField;

        private string currencyField;

        private string descriptionField;

        private decimal catNumberField;

        private PriceCatalogPriceCatHdrFilter[] filtersField;

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
        public decimal CatNumber
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
        [XmlArrayItem("Filter", IsNullable = false)]
        public PriceCatalogPriceCatHdrFilter[] Filters
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
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogPriceCatHdrFilter
    {

        private string filterIDField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
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
        [XmlAttribute()]
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
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
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
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogPriceCatHdrRouteFrom
    {

        private byte clientIDField;

        /// <remarks/>
        public byte ClientID
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
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogPriceCatHdrRouteTO
    {

        private uint clientIDField;

        /// <remarks/>
        public uint ClientID
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
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogCatalogItem
    {

        private PriceCatalogCatalogItemProduct productField;

        private PriceCatalogCatalogItemQty[] qtyField;

        private PriceCatalogCatalogItemPrice priceField;

        private object lvlPricingField;

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
        [XmlElementAttribute("Qty")]
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
        public object LvlPricing
        {
            get
            {
                return this.lvlPricingField;
            }
            set
            {
                this.lvlPricingField = value;
            }
        }
    }

    /// <remarks/>
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogCatalogItemProduct
    {

        private uint productIDField;

        private string partNumberField;

        private string eANCodeField;

        private string descriptionField;

        private string longDescField;

        private string periodofWarrantyField;

        private byte carePackField;

        private string vatTypeField;

        private decimal vatRateField;

        private PriceCatalogCatalogItemProductGroupBy[] groupingField;

        private PriceCatalogCatalogItemProductDimensions dimensionsField;

        private PriceCatalogCatalogItemProductWeight weightField;

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
        public byte CarePack
        {
            get
            {
                return this.carePackField;
            }
            set
            {
                this.carePackField = value;
            }
        }

        /// <remarks/>
        public string VatType
        {
            get
            {
                return this.vatTypeField;
            }
            set
            {
                this.vatTypeField = value;
            }
        }

        /// <remarks/>
        public decimal VatRate
        {
            get
            {
                return this.vatRateField;
            }
            set
            {
                this.vatRateField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("GroupBy", IsNullable = false)]
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

        /// <remarks/>
        public PriceCatalogCatalogItemProductDimensions Dimensions
        {
            get
            {
                return this.dimensionsField;
            }
            set
            {
                this.dimensionsField = value;
            }
        }

        /// <remarks/>
        public PriceCatalogCatalogItemProductWeight Weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }
    }

    /// <remarks/>
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogCatalogItemProductGroupBy
    {

        private string groupIDField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
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
        [XmlAttribute()]
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
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogCatalogItemProductDimensions
    {

        private string unitsField;

        private ushort heightField;

        private ushort widthField;

        private ushort lengthField;

        /// <remarks/>
        [XmlAttribute()]
        public string Units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public ushort Height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public ushort Width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public ushort Length
        {
            get
            {
                return this.lengthField;
            }
            set
            {
                this.lengthField = value;
            }
        }
    }

    /// <remarks/>
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogCatalogItemProductWeight
    {

        private string unitsField;

        private decimal grossField;

        /// <remarks/>
        [XmlAttribute()]
        public string Units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Gross
        {
            get
            {
                return this.grossField;
            }
            set
            {
                this.grossField = value;
            }
        }
    }

    /// <remarks/>
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogCatalogItemQty
    {

        private byte qtyAvailableField;

        private decimal unitPriceField;

        private bool unitPriceFieldSpecified;

        private System.DateTime dateArriveToLocalWHField;

        private bool dateArriveToLocalWHFieldSpecified;

        private string warehouseIDField;

        private string deliveryFromCountryField;

        private byte pSOmnivaField;

        private bool pSOmnivaFieldSpecified;

        private byte pSDPDField;

        private bool pSDPDFieldSpecified;

        private byte orderWarehouseIDField;

        private bool orderWarehouseIDFieldSpecified;

        private string deliveryTypeField;

        /// <remarks/>
        public byte QtyAvailable
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
        public decimal UnitPrice
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
        [XmlIgnoreAttribute()]
        public bool UnitPriceSpecified
        {
            get
            {
                return this.unitPriceFieldSpecified;
            }
            set
            {
                this.unitPriceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute(DataType = "date")]
        public System.DateTime DateArriveToLocalWH
        {
            get
            {
                return this.dateArriveToLocalWHField;
            }
            set
            {
                this.dateArriveToLocalWHField = value;
            }
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool DateArriveToLocalWHSpecified
        {
            get
            {
                return this.dateArriveToLocalWHFieldSpecified;
            }
            set
            {
                this.dateArriveToLocalWHFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string WarehouseID
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

        /// <remarks/>
        [XmlAttribute()]
        public string DeliveryFromCountry
        {
            get
            {
                return this.deliveryFromCountryField;
            }
            set
            {
                this.deliveryFromCountryField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public byte PSOmniva
        {
            get
            {
                return this.pSOmnivaField;
            }
            set
            {
                this.pSOmnivaField = value;
            }
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool PSOmnivaSpecified
        {
            get
            {
                return this.pSOmnivaFieldSpecified;
            }
            set
            {
                this.pSOmnivaFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public byte PSDPD
        {
            get
            {
                return this.pSDPDField;
            }
            set
            {
                this.pSDPDField = value;
            }
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool PSDPDSpecified
        {
            get
            {
                return this.pSDPDFieldSpecified;
            }
            set
            {
                this.pSDPDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public byte OrderWarehouseID
        {
            get
            {
                return this.orderWarehouseIDField;
            }
            set
            {
                this.orderWarehouseIDField = value;
            }
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool OrderWarehouseIDSpecified
        {
            get
            {
                return this.orderWarehouseIDFieldSpecified;
            }
            set
            {
                this.orderWarehouseIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DeliveryType
        {
            get
            {
                return this.deliveryTypeField;
            }
            set
            {
                this.deliveryTypeField = value;
            }
        }
    }

    /// <remarks/>
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogCatalogItemPrice
    {

        private PriceCatalogCatalogItemPriceUnitPrice unitPriceField;

        private byte promoFlagField;

        /// <remarks/>
        public PriceCatalogCatalogItemPriceUnitPrice UnitPrice
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
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class PriceCatalogCatalogItemPriceUnitPrice
    {

        private string typeField;

        private decimal valueField;

        /// <remarks/>
        [XmlAttribute()]
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
        [XmlTextAttribute()]
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
}
