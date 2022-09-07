using Stocker.Data;
using StockerDB.Data.Stocker;
using System.Net.Http.Json;
using MudBlazor;
using Stocker.API;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;


namespace Stocker.Pages;
public partial class Service
{
    [CascadingParameter]
    private Task<AuthenticationState>? authenticationStateTask { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Get the current user
        if (authenticationStateTask != null) {
            var user = (await authenticationStateTask).User;
            if (user.Identity != null) {
                //UserIdentityName = user.Identity.Name ?? "";
                //products = await @Service.GetAllProductsAsync();
                //await FilterChips();
            }
        }
    }
}
