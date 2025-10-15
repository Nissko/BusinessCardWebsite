using BusinessCardProject.Client;
using BusinessCardProject.Client.Services.ProjectInfoService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

/*Добавление библиотеки MudBlazor*/
builder.Services.AddMudServices();
/*Инициализация глобальных данных*/
builder.Services.AddSingleton<ProjectInfoGlobalClass>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
