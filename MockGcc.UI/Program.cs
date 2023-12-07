using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MockGcc.UI;
using MockGcc.UI.BackgroundServices;
using MockGcc.UI.ViewModels;

try
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.ConfigureMockGccApiClient(builder.HostEnvironment.BaseAddress);

    // Page ViewModels
    builder.Services.AddSingleton<IMainViewModel, MainViewModel>();

    builder.Services.AddSingleton<GetLatencyService>();
    builder.Services.AddHostedService<GetLatencyService>();

    builder.Services.AddSingleton<CallGetPersonInfoService>();
    builder.Services.AddHostedService<CallGetPersonInfoService>();

    builder.Services.AddSingleton<CallGetAccountService>();
    builder.Services.AddHostedService<CallGetAccountService>();
    
    var build = builder.Build();

    var getLatencyService = build.Services.GetRequiredService<GetLatencyService>();
    var callPersonInfoService = build.Services.GetRequiredService<CallGetPersonInfoService>();
    var callAccountService = build.Services.GetRequiredService<CallGetAccountService>();
    await getLatencyService.StartAsync(new CancellationToken());
    await callPersonInfoService.StartAsync(new CancellationToken());
    await callAccountService.StartAsync(new CancellationToken());

    await build.RunAsync();
}
catch
{
    // NLog: catch setup errors
    //logger.Error(exception, "MockGcc.UI could not start: Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    
}

