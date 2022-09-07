namespace Stocker.Parts
{
    public class StockObject
    {
        public string Warehouse { get; set; }
        public string PartNumber { get; set; }
        public decimal? PriceDiscounted { get; set; }
        public int? StockAvailable { get; set; }
        public int? StockIncoming { get; set; }
        public DateTime? DateIncoming { get; set; }
    }
}