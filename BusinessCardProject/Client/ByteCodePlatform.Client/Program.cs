using BusinessCardProject.Client;
using BusinessCardProject.Client.Services.AuthUserService;
using BusinessCardProject.Client.Services.ProjectInfoService;
using CourseService.Proto;
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
builder.Services.AddScoped<AuthDelegatingHandler>();
builder.Services.AddScoped<AuthInterceptor>();
builder.Services.AddScoped<ClientAuthService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddGrpcClient<CourseGrpcService.CourseGrpcServiceClient>(options =>
    {
        options.Address = new Uri("http://localhost:5221");
    })
    .ConfigurePrimaryHttpMessageHandler(sp =>
    {
        var tokenStore = sp.GetRequiredService<TokenStore>();
        var navManager = sp.GetRequiredService<NavigationManager>();
        var jsRuntime = sp.GetRequiredService<IJSRuntime>();
        var clientAuth = sp.GetRequiredService<ClientAuthService>();

        var authHandler = new AuthDelegatingHandler(tokenStore, navManager, jsRuntime, clientAuth)
        {
            InnerHandler = new HttpClientHandler()
        };

        return new GrpcWebHandler(GrpcWebMode.GrpcWeb, authHandler);
    })
    .AddInterceptor(sp => sp.GetRequiredService<AuthInterceptor>());

builder.Logging.SetMinimumLevel(LogLevel.Warning);
var host = builder.Build();
await host.Services.GetRequiredService<TokenStore>().InitializeAsync();
await host.RunAsync();