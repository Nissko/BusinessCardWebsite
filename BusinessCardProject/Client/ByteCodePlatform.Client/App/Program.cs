using BusinessCardProject.Client.App;
using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

/*Добавление библиотеки MudBlazor*/
builder.Services.AddMudServices();
builder.Services.AddMudBlazorResizeListener();

/*Инициализация кэша*/
builder.Services.AddSingleton<UserSettingService>();
builder.Services.AddSingleton<TokenStore>();
builder.Services.AddScoped<AuthenticationDelegatingHandler>();
builder.Services.AddScoped<AuthenticationInterceptor>();
builder.Services.AddScoped<ClientAuthenticationService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddGrpcClient<CourseService.Proto.CourseService.CourseServiceClient>(options =>
    {
        options.Address = new Uri("https://localhost:7117");
        //options.Address = new Uri("https://it-bytecode.splinterkeenetic.netcraze.club/CourseGrpcService");
    })
    .ConfigurePrimaryHttpMessageHandler(sp =>
    {
        var tokenStore = sp.GetRequiredService<TokenStore>();
        var navManager = sp.GetRequiredService<NavigationManager>();
        var jsRuntime = sp.GetRequiredService<IJSRuntime>();
        var clientAuth = sp.GetRequiredService<ClientAuthenticationService>();

        var authHandler = new AuthenticationDelegatingHandler(tokenStore, navManager, jsRuntime, clientAuth)
        {
            InnerHandler = new HttpClientHandler()
        };

        return new GrpcWebHandler(GrpcWebMode.GrpcWeb, authHandler);
    })
    .AddInterceptor(sp => sp.GetRequiredService<AuthenticationInterceptor>());

builder.Logging.SetMinimumLevel(LogLevel.Warning);
var host = builder.Build();
await host.Services.GetRequiredService<TokenStore>().InitializeAsync();
await host.RunAsync();