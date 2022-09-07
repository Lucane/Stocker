namespace Stocker.Parts.Lenovo.ApiObjects;

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


/*
{
  "ID": "LAPTOPS-AND-NETBOOKS/THINKPAD-T-SERIES-LAPTOPS/THINKPAD-T14-TYPE-20UD-20UE/20UD",
  "Type": "Product.MachineType",
  "Name": "T14  Gen 1 (type 20UD, 20UE) Laptop (ThinkPad) - Type 20UD",
  "Brand": "TPG",
  "Description": "20UD",
  "Specification": "",
  "Image": "https://download.lenovo.com/images/ProdImageLaptops/thinkpad_t14.jpg",
  "Released": "2000-01-01T00:00:00Z",
  "OperatingSystems": [
    "Windows 10 (64-bit)",
    "Windows 11 (64-bit)"
  ]
}
*/