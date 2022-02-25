using IdentityWasmProject;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using IdentityWasmProject.Authentication;
using IdentityWasmProject.Theme;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("IdentityProjectAPI")); // httpclient DI


builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 2000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();//sym

builder.Services.AddScoped<CustomAuthenticationState>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationState>();

builder.Services.AddSingleton<MudTheme, MudBlazorAdminDashboard>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("IdentityProjectAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7129");
});

await builder.Build().RunAsync();