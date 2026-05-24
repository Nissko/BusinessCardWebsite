using BusinessCardProject.Client;
using BusinessCardProject.Client.Services.ProjectInfoService;
using CourseService.Proto;
using Grpc.Net.Client.Web;
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

// builder.Services.AddSingleton(sp =>
// {
//     var channel = GrpcChannel.ForAddress("https://localhost:7117", new GrpcChannelOptions
//     {
//         HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
//     });
//     
//     return new CourseGrpcService.CourseGrpcServiceClient(channel);
// });

builder.Services.AddGrpcClient<CourseGrpcService.CourseGrpcServiceClient>(options =>
    {
        options.Address = new Uri("https://localhost:7117");
    })
    .ConfigurePrimaryHttpMessageHandler(() => 
        new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

await builder.Build().RunAsync();