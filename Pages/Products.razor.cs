using StockerDB.Data.Stocker;
using MudBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Stocker.Pages;

public partial class Products
{
    class Product
    {
        public int ID { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public int? TotalStockLocal { get; set; }
        public int? IncomingStockQty { get; set; }
        public DateTime? IncomingStockEarliest { get; set; }
        public decimal? PriceLocalMin { get; set; }
        public bool IsCarePack { get; set; }
    }

    [Inject] private NavigationManager NavManager { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? authenticationStateTask { get; set; }
    List<WarehouseProducts> products = new List<WarehouseProducts>();
    List<Product> filtered = new List<Product>();

    private string searchString = "";
    private bool _loading;
    private int showDetails = -1;
    MudChip[] selectedChips;

    // no need to limit access to this page as of now
    //private string UserIdentityName = "";

    protected override async Task OnInitializedAsync()
    {
        // Get the current user
        if (authenticationStateTask != null) {
            var user = (await authenticationStateTask).User;
            if (user.Identity != null) {
                //UserIdentityName = user.Identity.Name ?? "";
                //products = await @Service.GetAllProductsAsync();
                await FilterChips();
            }
        }
    }

    private async Task UpdateData()
    {
        var result = await @Service.PopulateDatabaseAsync();
    }

    private bool FilterFunc(Product element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.PartNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }

    private string ReturnIncomingDate(WarehouseProducts element)
    {
        if (element.Date_Incoming.HasValue) {
            var value = element.Stock_Incoming + "tk ";
            value += element.Date_Incoming.Value.ToString("yyyy-MM-dd");
            return value;
        } else {
            return "";
        }
    }

    private async Task FilterChips()
    {
        _loading = true;
        products = await @Service.GetAllProductsAsync();

        var newList = products.Where(x => selectedChips.Any(chip => ((chip.Text == "InStock" && x.Stock_Local > 0 && (x.CarePack ?? 0) == 0)
                                                                  || (chip.Text == "Incoming" && (x.Stock_Local ?? 0) == 0 && (x.Stock_Incoming > 0 || x.Date_Incoming != null) && (x.CarePack ?? 0) == 0)
                                                                  || (chip.Text == "OutOfStock" && (x.Stock_Local ?? 0) == 0 && (x.Stock_Incoming ?? 0) == 0 && (x.CarePack ?? 0) == 0)
                                                                  || (chip.Text == "ESD" && x.CarePack == 1))
                                                                  )).ToList();

        var convertedList = new List<Product>();

        foreach (var m in newList) {
            var matches = newList.Where(w => w.PartNumber == m.PartNumber);
            var incomingDate = matches.Min(x => x.Date_Incoming);
            var incomingQty = matches.Sum(x => x.Stock_Incoming);

            convertedList.Add(new Product {
                ID = m.Id,
                PartNumber = m.PartNumber,
                Description = m.Description,
                TotalStockLocal = matches.Select(x => x.Stock_Local).Sum(),
                IncomingStockEarliest = incomingDate,
                IncomingStockQty = incomingQty,
                PriceLocalMin = matches.Min(x => x.Price_Local),
                IsCarePack = matches.Any(x => x.CarePack == 1)
            });
        }

        convertedList = convertedList.GroupBy(g => g.PartNumber)
                                     .Select(s => s.First())
                                     .ToList();

        filtered = convertedList;
        _loading = false;
    }

    private string GetLink(WarehouseProducts cont)
    {
        return cont.PartNumber;
    }

    private async Task<string> ReturnTotalQty(WarehouseProducts element)
    {
        var matches = (await @Service.GetAllProductsAsync()).Where(x => x.PartNumber == element.PartNumber).ToList();

        int? total = 0;

        foreach (var m in matches) {
            total += m.Stock_Local;
        }

        return total.ToString();
    }

    private async Task<string> TTQTY(WarehouseProducts element)
    {
        var matches = (await @Service.GetAllProductsAsync()).Where(x => x.PartNumber == element.PartNumber).ToList();

        int? total = 0;

        foreach (var m in matches) {
            total += m.Stock_Local;
        }

        return total.ToString();
    }

    private void ShowBtnPress(TableRowClickEventArgs<Product> args)
    {
        if (args == null || args.Item == null) return;
        System.Diagnostics.Debug.WriteLine(args.Item.ID + " : " + showDetails);

        if (args.Item.ID == showDetails) {
            showDetails = -1;
            return;
        }

        showDetails = args.Item.ID;
    }

    private string ShowBtnPress2(Product element, int row)
    {
        if (element == null) return String.Empty;

        if (row == showDetails) {
            showDetails = -1;
            return String.Empty;
        }

        showDetails = row;
        return "row-selected";
    }
}
