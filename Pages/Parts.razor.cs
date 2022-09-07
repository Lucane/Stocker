using StockerDB.Data.Stocker;
using MudBlazor;
using Lenovo = Stocker.Parts.Lenovo;
using Flex = Stocker.Parts.Flex;
using Stocker.Parts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Stocker.Pages;
public partial class Parts
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IJSRuntime jsRuntime { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? authenticationStateTask { get; set; }

    private MudChip[]? _selectedChips { get; set; }
    private MudChip[]? _selectedChipsModel
    {
        get => _selectedChips;
        set => _selectedChips = value;
    }

    List<LenovoDevices> products = new List<LenovoDevices>();
    LenovoDevices? _device = null;
    string? _searchInput = "";

    List<Lenovo.ParsedObject>? modelParts = new List<Lenovo.ParsedObject>();
    List<Lenovo.ParsedObject>? compatibleParts = new List<Lenovo.ParsedObject>();
    List<StockObject>? stockStatus = new List<StockObject>();

    List<Product> partList = new List<Product>();
    List<Product> filteredList = new List<Product>();

    private bool _tableLoading;
    private string showDetails = "";
    
    private string searchString = "";
    private bool showImageOverlay;
    private bool _showLoadingOverlay;
    private string showImages = "";
    private bool _showTable = false;
    private int _activePanelIndex = 0;

    private IEnumerable<string>? _selectedCategories { get; set; }

    class Product
    {
        public bool IsModelPart { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int? TotalStock { get; set; }
        public DateTime? IncomingStockEarliest { get; set; }
        public decimal? PriceMin { get; set; }
        public List<string>? ImageURLs { get; set; }
    }

    protected override async Task OnInitializedAsync()
    {
        // Get the current user
        if (authenticationStateTask != null) {
            var user = (await authenticationStateTask).User;
            if (user.Identity != null) {
                products = await @Service.GetAllProductsAsync();

                //UserIdentityName = user.Identity.Name ?? "";
                
                //await FilterChips();
            }
            
            //if (!user.Identity!.IsAuthenticated) {
            //    NavManager.NavigateTo($"Identity/Account/Login");
            //}
        }
    }

    private async Task<IEnumerable<LenovoDevices>?> SearchFunc (string value)
    {
        if (String.IsNullOrWhiteSpace(value) || value.Length < 2)
            return null;

        return products.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private bool FilterFunc(Product element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.PartNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        //if ($"{element.Number} {element.Position} {element.Molar}".Contains(searchString))
        //    return true;

        return false;
    }

    private async Task PopulateImageOverlay(string partNumber)
    {
        _showLoadingOverlay = true;

        showDetails = "";
        showImages = partNumber;
        showImageOverlay = true;

        _showLoadingOverlay = false;
    }

    private async Task ShowRowDetails(string partNumber)
    {
        if (partNumber == null || partNumber == "") return;

        if (partNumber == showDetails) {
            showDetails = "";
            return;
        }

        showDetails = partNumber;
    }

    private async Task SearchCustomInput(KeyboardEventArgs e)
    {
        if (e.Code != "Enter" && e.Code != "NumpadEnter") return;
        if (String.IsNullOrWhiteSpace(_searchInput) || _showLoadingOverlay) return;

        _showTable = false;
        _showLoadingOverlay = true;
        StateHasChanged();

        var details = await API.Lenovo.Product.GetDetailsAsync(_searchInput);
        //System.Diagnostics.Debug.WriteLine(":: SearchCustomInput => " + _searchInput + " => " + details?.Type + " => " + details?.ID);

        if (details is null) {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Sellist tootekoodi või seerianumbrit ei eksisteeri!");
            parameters.Add("DisableCancelButton", true);

            DialogService.Show<Shared.MudDialogTemplate>("Veateade", parameters);

            _showLoadingOverlay = false;
            return;
        }

        var combinedURL = "https://pcsupport.lenovo.com/ee/en/products/" + details.ID + "/parts/display";

        if (details.Type == "Product.Serial") await PopulateTable(combinedURL + "/as-built");
        else if (details.Type == "Product.Model") await PopulateTable(combinedURL + "/model");
        else if (details.Type == "Product.MachineType") await PopulateTable(combinedURL + "/compatible");
    }

    private async Task DeviceTextChanged(string value)
    {
        _searchInput = value;
    }

    private async Task SelectedChipsChanged(MudChip[] chips)
    {
        _selectedChips = chips;
    }

    private async Task DeviceSelectionChanged(LenovoDevices dev)
    {
        if (dev is null) return;
        await PopulateTable(dev.ProductURL);
        //System.Diagnostics.Debug.WriteLine(":: DeviceSelectionChanged => " + dev.Name);
    }

    private async Task FiltersChanged()
    {
        if (partList is null) return;
        _tableLoading = true;

        var newList = partList.Where(x => _selectedCategories.Contains(x.Category));

        if (newList is null || _selectedChips is null) {
            filteredList = newList?.ToList() ?? new List<Product>();
            _tableLoading = false;
            return;
        }

        newList = newList.Where(x => _selectedChips.Any(chip => ((chip.Text == "InStock" && x.TotalStock > 0)
                                                                   || (chip.Text == "Incoming" && (x.TotalStock ?? 0) == 0 && x.IncomingStockEarliest.HasValue)
                                                                   || (chip.Text == "OutOfStock" && (x.TotalStock ?? 0) == 0 && (!x.IncomingStockEarliest.HasValue || x.IncomingStockEarliest == DateTime.MinValue)))
                                                                   ));

        filteredList = newList.ToList();
        _tableLoading = false;
    }

    private async Task PopulateTable(string fullURL)
    {
        //if (_device is null) return;

        _showTable = false;
        _showLoadingOverlay = true;
        modelParts = null;
        compatibleParts = null;
        _selectedCategories = null;
        _activePanelIndex = 1;

        StateHasChanged();

        var UrlWithoutLastPath = Regex.Replace(fullURL, @"\/(compatible|as-built|model)", "");

        if (fullURL.Contains("/as-built")
            || fullURL.Contains("/model")) {

            modelParts = await Lenovo.ApiCaller.GetParts(UrlWithoutLastPath + "/model");
            _activePanelIndex = 0;
        }

        compatibleParts = await Lenovo.ApiCaller.GetParts(UrlWithoutLastPath + "/compatible");

        if (compatibleParts is null) {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Varuosade nimekirja päring ebaõnnestus!");
            parameters.Add("DisableCancelButton", true);

            DialogService.Show<Shared.MudDialogTemplate>("Veateade", parameters);

            _showLoadingOverlay = false;
            return;
        }

        stockStatus = await Flex.ApiCaller.GetStockStatus(compatibleParts.Select(x => x.PartNumber).ToList());

        if (stockStatus is null) {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Varuosade laoseisu päring ebaõnnestus!");
            parameters.Add("DisableCancelButton", true);

            DialogService.Show<Shared.MudDialogTemplate>("Veateade", parameters);

            _showLoadingOverlay = false;
            return;
        }

        foreach (var m in compatibleParts) {
            //var matches = parts.Where(x => x.PartNumber == m.PartNumber);
            //var incomingDate = matches.Min(x => x.Date_Incoming);
            //var incomingQty = matches.Sum(x => x.Stock_Incoming);

            //var priceMin = matches.Any(x => x.Stock_Local > 0). Min(x => x.Price_Local);

            var stockMatches = stockStatus?.Where(x => x.PartNumber == m.PartNumber);
            if (stockMatches is null) continue;

            partList.Add(new Product {
                IsModelPart = modelParts?.Any(x => x.PartNumber == m.PartNumber) ?? false,
                Category = m?.Category ?? "",
                PriceMin = stockMatches.Min(x => (decimal?)x.PriceDiscounted),
                TotalStock = stockMatches.Sum(x => (int?)x.StockAvailable),
                IncomingStockEarliest = stockMatches.Where(x => x.DateIncoming != DateTime.MinValue).Min(x => (DateTime?)x.DateIncoming),
                PartNumber = m?.PartNumber ?? "",
                Description = m?.Description ?? "",
                ImageURLs = m.ImageURLs
                //TotalStock = matches.Select(x => x.Stock_Local).Sum(),
                //PriceLocalMin = matches.Min(x => x.Price_Local),
            });
        }

        _selectedCategories = partList.Select(x => x.Category).Distinct();
        filteredList = partList;

        _showTable = true;
        _showLoadingOverlay = false;

        StateHasChanged();
    }

    public async Task NavigateToNewTab(string partNumber)
    {
        string url = @"https://www.lenovopartsales.com/LenovoEsales/ccrz__Products?operation=quickSearch&searchText=" + partNumber;
        await jsRuntime.InvokeVoidAsync("open", url, "_blank");
    }

    private async Task FilterChips()
    {
        if (_device is null) return;
        _tableLoading = true;

        if (compatibleParts is null || compatibleParts?.Count() == 0) {
            _showTable = false;
            //var UrlWithoutLastPath = String.Join(String.Empty,
            //    new Uri(_device.ProductURL).AbsoluteUri.ToString() +
            //    new Uri(_device.ProductURL).Segments.SkipLast(1).ToList());

            var UrlWithoutLastPath = Regex.Replace(_device.ProductURL, @"\/(compatible|as-built|model)", "");

            if (_device.ProductURL.Contains("/as-built")
                || _device.ProductURL.Contains("/model")) {

                modelParts = await Lenovo.ApiCaller.GetParts(UrlWithoutLastPath + "/model");
            }
            
            compatibleParts = await Lenovo.ApiCaller.GetParts(UrlWithoutLastPath + "/compatible");
        } else {
            var newList = partList.Where(x => _selectedChips.Any(chip => ((chip.Text == "InStock" && x.TotalStock > 0)
                                                                      || (chip.Text == "Incoming" && (x.TotalStock ?? 0) == 0 && x.IncomingStockEarliest.HasValue)
                                                                      || (chip.Text == "OutOfStock" && (x.TotalStock ?? 0) == 0 && (!x.IncomingStockEarliest.HasValue || x.IncomingStockEarliest == DateTime.MinValue)))
                                                                      ));

            newList = newList.Where(x => _selectedCategories.Contains(x.Category));

            filteredList = newList.ToList();
            _tableLoading = false;
            return;
        }


        if (compatibleParts is null) return;

        stockStatus = await Flex.ApiCaller.GetStockStatus(compatibleParts.Select(x => x.PartNumber).ToList());

        partList = new List<Product>();
        //var newList = parts;

        foreach (var m in compatibleParts) {
            //var matches = parts.Where(x => x.PartNumber == m.PartNumber);
            //var incomingDate = matches.Min(x => x.Date_Incoming);
            //var incomingQty = matches.Sum(x => x.Stock_Incoming);

            //var priceMin = matches.Any(x => x.Stock_Local > 0). Min(x => x.Price_Local);

            var stockMatches = stockStatus?.Where(x => x.PartNumber == m.PartNumber);
            if (stockMatches is null) continue;

            partList.Add(new Product {
                IsModelPart = m.IsModelPart,
                Category = m.Category,
                PriceMin = stockMatches.Min(x => (decimal?) x.PriceDiscounted),
                TotalStock = stockMatches.Sum(x => (int?) x.StockAvailable),
                IncomingStockEarliest = stockMatches.Where(x => x.DateIncoming != DateTime.MinValue).Min(x => (DateTime?) x.DateIncoming),
                PartNumber = m.PartNumber,
                Description = m.Description,
                ImageURLs = m.ImageURLs
                //TotalStock = matches.Select(x => x.Stock_Local).Sum(),
                //PriceLocalMin = matches.Min(x => x.Price_Local),
            });
        }

        filteredList = partList.Where(x => _selectedChips.Any(chip => ((chip.Text == "InStock" && x.TotalStock > 0)
                                                                  || (chip.Text == "Incoming" && x.TotalStock == 0 && x.IncomingStockEarliest.HasValue)
                                                                  || (chip.Text == "OutOfStock" && x.TotalStock == 0 && !x.IncomingStockEarliest.HasValue))
                                                                  )).ToList();

        _selectedCategories = filteredList.Select(x => x.Category).Distinct();

        _tableLoading = false;
    }
}
