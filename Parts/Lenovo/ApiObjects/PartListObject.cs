namespace Stocker.Parts.Lenovo.ApiObjects;


public class PartObject
{
    public string ID { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Level { get; set; }
}

/* <- PartObject ->
[
  {
    "ID": "(PPN)43Y3944",
    "Type": null,
    "Name": "",
    "Level": "SERIAL"
  },
  {
    "ID": "(PPN)NA",
    "Type": null,
    "Name": "",
    "Level": "SERIAL"
  },
  {
    "ID": "00UP735",
    "Type": "M.2 Card",
    "Name": "512G,M.2,2280,PCIe3x4,SAM,OPAL",
    "Level": "SERIAL"
  },
  {
    "ID": "02HK704",
    "Type": "Wireless LAN adapters",
    "Name": "Wireless,CMB,IN,22260 vPro",
    "Level": "SERIAL"
  }
]
*/




public class PartObjectDetailed
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public List<string> Images { get; set; }
    public List<Substitute> Substitutes { get; set; }
}

public class Substitute
{
    public string Type { get; set; }
    public string Part { get; set; }
}


/* <- PartObjectDetailed ->
{
  "ID": "00UP735",
  "Name": "512G,M.2,2280,PCIe3x4,SAM,OPAL",
  "Description": "",
  "Type": "M.2 Card",
  "Images": [
    "~/FileStreamer?File=/Images/Parts/00UP735/00UP735_A.jpg",
    "~/FileStreamer?File=/Images/Parts/00UP735/00UP735_B.jpg",
    "~/FileStreamer?File=/Images/Parts/00UP735/00UP735_box.jpg",
    "~/FileStreamer?File=/Images/Parts/00UP735/00UP735_C.jpg",
    "~/FileStreamer?File=/Images/Parts/00UP735/00UP735_D.jpg",
    "~/FileStreamer?File=/Images/Parts/00UP735/00UP735_details.jpg",
    "~/FileStreamer?File=/Images/Parts/00UP735/00UP735_E.jpg",
    "~/FileStreamer?File=/Images/Parts/00UP735/00UP735_F.jpg",
    "~/FileStreamer?File=/Images/Parts/00UP735/00UP735_label.jpg"
  ],
  "Substitutes": [
    {
      "Type": "PPN_FOR",
      "Part": "SBB0L16222"
    },
    {
    "Type": "SOFT_FOR",
      "Part": "00JT032"
    },
    {
    "Type": "EQUIVALENT",
      "Part": "00UP435"
    },
    {
    "Type": "SOFT",
      "Part": "00UP438"
    }
  ]
}
*/