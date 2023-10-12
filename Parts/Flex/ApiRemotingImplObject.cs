using System.Text.Json.Serialization;

namespace Stocker.Parts.Flex;

public class ApiRemotingImplObject
{
    public Vf vf { get; set; }
    public Actions actions { get; set; }
    public string service { get; set; }
}

public class Vf
{
    public string vid { get; set; }
    public bool xhr { get; set; }
    public bool dev { get; set; }
    public bool tst { get; set; }
    public bool dbg { get; set; }
    public long tm { get; set; }
    public bool ovrprm { get; set; }
}

public class Actions
{
    [JsonPropertyName("cc_lenovo_ctrl_ProductListShowProduct")]
    public ProductListShowProduct productListShowProduct { get; set; }

    [JsonPropertyName("cc_lenovo_ctrl_QuickOrderAlternatives")]
    public QuickOrderAlternatives quickOrderAlternatives { get; set; }

    [JsonPropertyName("ccrz.cc_ctrl_BreadCrumb")]
    public BreadCrumb breadCrumb { get; set; }

    [JsonPropertyName("ccrz.cc_ctrl_CartRD")]
    public CartRd cartRd { get; set; }

    [JsonPropertyName("ccrz.cc_ctrl_FeatureFilterRD")]
    public FeatureFilterRd featureFilterRd { get; set; }

    [JsonPropertyName("ccrz.cc_ctrl_Header")]
    public Header header { get; set; }

    [JsonPropertyName("ccrz.cc_ctrl_MenuBar")]
    public MenuBar menuBar { get; set; }

    [JsonPropertyName("ccrz.cc_ctrl_ProductListRD")]
    public ProductListRd productListRd { get; set; }

    [JsonPropertyName("ccrz.cc_ctrl_promoRD")]
    public PromoRd promoRD { get; set; }

    [JsonPropertyName("ccrz.cc_RemoteActionController")]
    public RemoteActionController remoteActionController { get; set; }
}

public class M
{
    public string name { get; set; }
    public int len { get; set; }
    public string ns { get; set; }
    public float ver { get; set; }
    public string csrf { get; set; }
    public string authorization { get; set; }
}

public class ProductListShowProduct
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}

public class QuickOrderAlternatives
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}

public class BreadCrumb
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}

public class CartRd
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}

public class FeatureFilterRd
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}

public class Header
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}

public class MenuBar
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}

public class ProductListRd
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}

public class PromoRd
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}

public class RemoteActionController
{
    public M[] ms { get; set; }
    public int prm { get; set; }
}