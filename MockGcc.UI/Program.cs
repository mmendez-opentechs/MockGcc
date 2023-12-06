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

    var build = builder.Build();

    var hostedService = build.Services.GetRequiredService<GetLatencyService>();
    await hostedService.StartAsync(new CancellationToken());

    await build.RunAsync();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    //logger.Error(exception, "MockGcc.UI could not start: Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    
}

