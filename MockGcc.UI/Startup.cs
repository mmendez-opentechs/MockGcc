﻿using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MockGcc.UI.HttpClients;

namespace MockGcc.UI
{
    public static class Startup
    {
        public static void ConfigureMockGccApiClient(this IServiceCollection services, string baseAddress)
        {
            services.AddHttpClient<MockGccServiceClient>().ConfigureHttpClient((client) =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
        }
    }
}
