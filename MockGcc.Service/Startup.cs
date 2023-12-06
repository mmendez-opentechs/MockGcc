using MockGcc.Service.BackgroundServices;
using MockGcc.Service.HttpClients;
using MockGcc.Service.State;
using NLog.Extensions.Logging;

namespace MockGcc.Service
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        private IConfigurationRoot _configurationRoot;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;

            #region Configure Logging
            // Get the factory for ILogger instances.
            NLogLoggerProvider nlogLoggerProvider = new NLogLoggerProvider();
            // Create an ILogger.
            _logger = nlogLoggerProvider.CreateLogger(typeof(Startup).FullName);
            #endregion

            LoadConfigurationFiles();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogDebug("ConfigureServices method Begin");

            ConfigureSettings(services);

            ConfigureState(services);

            ConfigureMockClients(services);

            ConfigureBackgroundServices(services);

            _logger.LogDebug("ConfigureServices method End");
        }

        private void ConfigureState(IServiceCollection services)
        {
            var frequencyState = new State.State()
            {
                MockAccountRate = 1,
                MockPersonInfoRate = 1
            };

            services.AddSingleton(frequencyState);
        }

        #region Private Methods
        private void ConfigureSettings(IServiceCollection services)
        {
            _logger.LogDebug("Settings Configured...");

            services.AddSingleton(_logger);
        }

        private void ConfigureMockClients(IServiceCollection services)
        {
            var mockPersonInfoBaseAddress = Environment.GetEnvironmentVariable("MOCK_PERSONINFO_BASEADDRESS");
            mockPersonInfoBaseAddress = mockPersonInfoBaseAddress ?? "https://127.0.0.1:7086";
            var mockAccountBaseAddress = Environment.GetEnvironmentVariable("MOCK_ACCOUNT_BASEADDRESS");
            mockAccountBaseAddress = mockAccountBaseAddress ?? "https://127.0.0.1:7087";

            services.AddHttpClient<MockPersonInfoClient>().ConfigureHttpClient((client) =>
            {
                client.BaseAddress = new Uri(mockPersonInfoBaseAddress);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };
                return httpClientHandler;
            });

            services.AddHttpClient<MockAccountClient>().ConfigureHttpClient((client) =>
            {
                client.BaseAddress = new Uri(mockAccountBaseAddress);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };
                return httpClientHandler;
            });
        }

        private void ConfigureBackgroundServices(IServiceCollection services)
        {
            services.AddHostedService<MockServiceRequester>();
            
        }

        private void LoadConfigurationFiles()
        {
            #region Config Files
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(_env.ContentRootPath);

            builder.AddJsonFile($@"Config\appConfig.json", optional: false, reloadOnChange: false);

            _configurationRoot = builder.Build();
            _logger.LogDebug("Configuration files loaded.");
            #endregion
        }
        #endregion
    }
}
