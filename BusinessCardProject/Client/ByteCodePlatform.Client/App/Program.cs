using BusinessCardProject.Client.App;
using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

/* Добавление библиотеки MudBlazor */
builder.Services.AddMudServices();
builder.Services.AddMudBlazorResizeListener();

/* Инициализация кэша и сервисов */
builder.Services.AddScoped<UserSettingService>();
builder.Services.AddSingleton<TokenStore>();
builder.Services.AddScoped<AuthenticationInterceptor>();
builder.Services.AddScoped<ClientAuthenticationService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("AuthRefreshClient", client =>
    {
        client.BaseAddress = new Uri("https://localhost:7241");
        //client.BaseAddress = new Uri("https://it-bytecode.splinterkeenetic.netcraze.club/AuthGrpcService");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
        new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
    );

HttpMessageHandler CreateAuthHandler(IServiceProvider sp)
{
    var authHandler = new AuthenticationDelegatingHandler(
        sp,
        sp.GetRequiredService<ILogger<AuthenticationDelegatingHandler>>()
    )
    {
        InnerHandler = new HttpClientHandler()
    };

    return new GrpcWebHandler(GrpcWebMode.GrpcWeb, authHandler);
}

builder.Services.AddGrpcClient<AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient>(options =>
    {
        options.Address = new Uri("https://localhost:7241");
        //options.Address = new Uri("https://it-bytecode.splinterkeenetic.netcraze.club/AuthGrpcService");
    })
    .ConfigurePrimaryHttpMessageHandler(CreateAuthHandler);

builder.Services.AddGrpcClient<CourseService.Proto.CourseService.CourseServiceClient>(options =>
    {
        options.Address = new Uri("https://localhost:7117");
        //options.Address = new Uri("https://it-bytecode.splinterkeenetic.netcraze.club/CourseGrpcService");
    })
    .ConfigurePrimaryHttpMessageHandler(CreateAuthHandler)
    .AddInterceptor(sp => sp.GetRequiredService<AuthenticationInterceptor>());

builder.Logging.SetMinimumLevel(LogLevel.Warning);

var host = builder.Build();

await host.Services.GetRequiredService<TokenStore>().InitializeAsync();
var userSettings = host.Services.GetRequiredService<UserSettingService>();
await userSettings.Load();
userSettings.SetAuthorizationGrpcClient(host.Services
    .GetRequiredService<AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient>());
userSettings.SetTokenStorage(host.Services.GetRequiredService<TokenStore>());

var auth = host.Services.GetRequiredService<ClientAuthenticationService>();
if (!await auth.ValidateAndClearAsync())
{
    var navManager = host.Services.GetRequiredService<NavigationManager>();
    navManager.NavigateTo("/login", forceLoad: true);
}

await host.RunAsync();