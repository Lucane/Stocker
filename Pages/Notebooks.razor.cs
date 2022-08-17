using Stocker.Data;
using StockerDB.Data.Stocker;
using System.Net.Http.Json;
using MudBlazor;
using Stocker.API;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using Microsoft.JSInterop;


namespace Stocker.Pages;

public partial class Notebooks
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

    // AuthenticationState is available as a CascadingParameter
    [CascadingParameter]
    private Task<AuthenticationState>? authenticationStateTask { get; set; }
    //List<WarehouseConnection> warehouses = new List<WarehouseConnection>();
    List<WarehouseProducts> products = new List<WarehouseProducts>();
    List<Product> filtered = new List<Product>();

    private string searchString = "";
    //MudChip[] selectedChips = new MudChip[chipStockACC, chipStockALSO, chipStockF9, chipInStock];
    private bool inStock = true;
    private bool stockACC = true;
    private bool stockALSO = true;
    private bool stockF9 = true;
    private bool _loading;
    private int showDetails = -1;
    MudChip[] selectedChips;
    private int? totalQty = 0;

    // no need to limit access to this page as of now
    //private string UserIdentityName = "";

    protected override async Task OnInitializedAsync()
    {
        //System.Diagnostics.Debug.WriteLine("------------------- Initializing page 'notebooks'");

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

    //WarehouseProductsAlso objProducts = new WarehouseProductsAlso();
    //bool ShowPopup = false;
    //bool ShowRemovalPopup = false;

    //void ClosePopup()
    //{
    //    // Close the Popup
    //    ShowPopup = false;
    //    ShowRemovalPopup = false;
    //}

    //void AddWarehouse()
    //{
    //    objWarehouse = new WarehouseProductsAlso();
    //    objWarehouse.Id = 0;
    //    ShowPopup = true;
    //}

    //async Task SaveWarehouse()
    //{
    //    // Close the Popup
    //    ShowPopup = false;
    //    // A new forecast will have the Id set to 0
    //    if (objWarehouse.Id == 0)
    //    {
    //        // Create new forecast
    //        var objNewWarehouse = new WarehouseConnection();
    //        objNewWarehouse.DisplayName = objWarehouse.DisplayName;
    //        objNewWarehouse.ConnectionUri = objWarehouse.ConnectionUri;
    //        objNewWarehouse.LoginName = objWarehouse.LoginName;
    //        objNewWarehouse.LoginSecret = objWarehouse.LoginSecret;
    //        objNewWarehouse.LastUpdated = System.DateTime.Now;

    //        // Save the result
    //        var result = @Service.CreateWarehouseAsync(objNewWarehouse);
    //    }
    //    else
    //    {
    //        var result = @Service.UpdateWarehouseAsync(objWarehouse);
    //    }

    //    // Get the forecasts for the current user
    //    warehouses = await @Service.GetWarehousesAsync();
    //}

    //void EditWarehouse(WarehouseConnection warehouse)
    //{
    //    objWarehouse = warehouse;
    //    ShowPopup = true;
    //}

    //void ConfirmRemoval(WarehouseConnection warehouse)
    //{
    //    objWarehouse = warehouse;
    //    ShowRemovalPopup = true;
    //}

    //async Task DeleteWarehouse()
    //{
    //    var result = @Service.DeleteWarehouseAsync(objWarehouse);
    //    warehouses = await @Service.GetWarehousesAsync();
    //    ShowRemovalPopup = false;
    //}

    private async Task UpdateData()
    {
        //var result = await @Service.PopulateDatabaseAsync();
        //var result = API.ALSO.Xml.GetXmlRequest(API.ALSO.Xml.Warehouse.ACC);
        //var result = await API.ACC.ApiCaller.GetProducts();
        //var result = await API.F9.ApiCaller.GetProducts();
        var result = await @Service.PopDB();
        //System.Diagnostics.Debug.WriteLine(result);
    }

    private bool FilterFunc(Product element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.PartNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        //if ($"{element.Number} {element.Position} {element.Molar}".Contains(searchString))
        //    return true;

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
        //foreach (var chip in selectedChips) {
        //foreach (var c in selectedChips) {
        //    System.Diagnostics.Debug.WriteLine(c.Text + " : " + c.IsSelected + products.ElementAt(0).CarePack);
        //}

        //products = await @Service.GetAllProductsAsync();


        //foreach (var product in await @Service.GetAllProductsAsync()) {

        //}

        products = await @Service.GetAllProductsAsync();

        var newList = products.Where(x => selectedChips.Any(chip => ((chip.Text == "InStock" && x.Stock_Local > 0 && (x.CarePack ?? 0) == 0)
                                                                  || (chip.Text == "Incoming" && (x.Stock_Local ?? 0) == 0 && (x.Stock_Incoming > 0 || x.Date_Incoming != null) && (x.CarePack ?? 0) == 0)
                                                                  || (chip.Text == "OutOfStock" && (x.Stock_Local ?? 0) == 0 && (x.Stock_Incoming ?? 0) == 0 && (x.CarePack ?? 0) == 0)
                                                                  || (chip.Text == "ESD" && x.CarePack == 1))
                                                                  )).ToList();

        
        //newList = newList.Where(x => selectedChips.Any(chip => ((chip.Text == "ACC" && x.Warehouse == "ACC")
        //                                                     || (chip.Text == "ALSO" && x.Warehouse == "ALSO")
        //                                                     || (chip.Text == "F9" && x.Warehouse == "F9"))
        //                                                     )).ToList();

        //newList = newList.GroupBy(g => g.PartNumber)
        //                 .Select(s => s.OrderBy(x => x.Price_Local).First())
        //                 .ToList();

        var convertedList = new List<Product>();

        foreach (var m in newList) {
            var matches = newList.Where(w => w.PartNumber == m.PartNumber);
            var incomingDate = matches.Min(x => x.Date_Incoming);
            var incomingQty = matches.Sum(x => x.Stock_Incoming);

            //var priceMin = matches.Any(x => x.Stock_Local > 0). Min(x => x.Price_Local);

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

        //filtered = products.ConvertAll(x => new Product {
        //    PartNumber = x.PartNumber,
        //    Description = x.Description,

        //});

        //foreach (var product in newList) {

        //}
        //List<WarehouseProducts> repeatingValues
        //newList = newList.GroupBy(g => g.PartNumber)
        //                 .Select(s => s.OrderBy(x => x.Price_Local).First())
        //                 .ToList();

        //products = newList;
        //products = (await @Service.GetAllProductsAsync()).Where(x => selectedChips.Any(chip => ((chip.Text == "InStock" && x.Stock_Local > 0 && x.CarePack == 0)
        //                                                                                    || (chip.Text == "Incoming" && x.Stock_Incoming > 0 && x.CarePack == 0)
        //                                                                                    || (chip.Text == "OutOfStock" && x.Stock_Local == 0 && x.CarePack == 0)
        //                                                                                    || (chip.Text == "ESD" && x.CarePack == 1))
        //                                                                                    )).ToList();
        //&& (
        //   (chip.Text == "ACC" && x.Warehouse == "ACC")
        //|| (chip.Text == "ALSO" && x.Warehouse == "ALSO")
        //|| (chip.Text == "F9" && x.Warehouse == "F9")
        //)

        //products = products.Where(x => chip.IsSelected && chip.Text == "OutOfStock" && x.Stock_EE == 0).ToList();
        //}
        //if (inStock) products = products.Where(x => x.Stock_EE > 0).ToList();

        await Task.Delay(50);
        _loading = false;
    }

    private string GetLink(WarehouseProducts cont)
    {
        return cont.PartNumber;
        //return PartNo;
        //var matches = (@Service.GetAllProductsAsync().Result).Where(x => x.PartNumber == PartNo).ToList();

        //int? total = 0;

        //foreach (var m in matches) {
        //    total += m.Stock_Local;
        //}

        //return total.ToString();
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
        //WarehouseProducts_ALSO tmpPerson = products.First(x => x.Id == ID);
        //showDetails = ;
    }

    private string ShowBtnPress2(Product element, int row)
    {
        if (element == null) return String.Empty;
        //System.Diagnostics.Debug.WriteLine(args.Item.ID + " : " + showDetails);

        if (row == showDetails) {
            showDetails = -1;
            return String.Empty;
        }

        showDetails = row;
        return "row-selected";
        //WarehouseProducts_ALSO tmpPerson = products.First(x => x.Id == ID);
        //showDetails = ;
    }
}
