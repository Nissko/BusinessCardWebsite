using BusinessCardProject.Client;
using BusinessCardProject.Client.Services.ProjectInfoService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

/*Добавление библиотеки MudBlazor*/
builder.Services.AddMudServices();
builder.Services.AddMudBlazorResizeListener();
/*Инициализация кэша*/
builder.Services.AddSingleton<UserSettingService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
