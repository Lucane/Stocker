namespace Stocker.Parts;

//public class ProductObject
//{
//    public Class1[] Products { get; set; }
//}

public class ProductObject
{
    public string Id { get; set; }
    public string Brand { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string ProductGuid { get; set; }
    public string Type { get; set; }
    public string ParentID { get; set; }
    public string FullGuid { get; set; }
    public string Popularity { get; set; }
}

public class CompressedObject
{
    public string content { get; set; }
    public int originLength { get; set; }
}