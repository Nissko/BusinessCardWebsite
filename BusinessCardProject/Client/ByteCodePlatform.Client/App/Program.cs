using BusinessCardProject.Client.App;
using BusinessCardProject.Client.Entities.Services.Mains;
using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.Development.json");

var config = builder.Configuration
             ?? throw new InvalidOperationException("Configuration is not initialized.");

var authServiceUrl = config["Services:AuthorizationServiceUrl"]
              ?? throw new InvalidOperationException("AuthorizationServiceUrl is not initialized.");

var courseServiceUrl = config["Services:CourseServiceBaseUrl"]
                ?? throw new InvalidOperationException("CourseServiceBaseUrl is not initialized.");

var fileServiceUrl = config["Services:FileServiceBaseUrl"]
                ?? throw new InvalidOperationException("FileServiceBaseUrl is not initialized.");

builder.Services.AddMudServices();
builder.Services.AddMudBlazorResizeListener();

/* Инициализация кэша и сервисов */
builder.Services.AddScoped<UserSettingService>();
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<GetLinksService>();
builder.Services.AddSingleton<TokenStore>();
builder.Services.AddScoped<AuthenticationInterceptor>();
builder.Services.AddScoped<ClientAuthenticationService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("AuthRefreshClient", client => client.BaseAddress = new Uri(authServiceUrl))
    .ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

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
        options.Address = new Uri(authServiceUrl);
    })
    .ConfigurePrimaryHttpMessageHandler(CreateAuthHandler);

builder.Services.AddGrpcClient<FilesService.Proto.FilesService.FilesServiceClient>(options =>
    {
        options.Address = new Uri(fileServiceUrl);
    })
    .ConfigurePrimaryHttpMessageHandler(CreateAuthHandler)
    .AddInterceptor(sp => sp.GetRequiredService<AuthenticationInterceptor>());

builder.Services.AddGrpcClient<CourseService.Proto.CourseService.CourseServiceClient>(options =>
    {
        options.Address = new Uri(courseServiceUrl);
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
if (!await auth.ValidateTokenOnLoad())
{
    var navManager = host.Services.GetRequiredService<NavigationManager>();
    navManager.NavigateTo("/login", forceLoad: true);
}

await host.RunAsync();