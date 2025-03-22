using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using StudentMarksheet.ApiService;
using StudentMarksheet.DataAccess;
using StudentMarksheet.Model;
using StudentMarksheet.Services;
using StudentMarksheet.ViewModel;
using System.Configuration;
using System.Windows;

namespace StudentMarksheet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Configure Logging
                ConfigureLogging();

                // Configure Services (Dependency Injection)
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                _serviceProvider = serviceCollection.BuildServiceProvider();

                // Global Exception Handling
                ConfigureGlobalExceptionHandling();

                // Launch Main Window
                var mainWindow = _serviceProvider.GetRequiredService<StudentMarksheetView>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start.");
                MessageBox.Show("A fatal error occurred. Please check the log file.", "Critical Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1); // Exit application on fatal error
            }
        }

        /// <summary>
        /// Configures Dependency Injection Services
        /// </summary>
        private void ConfigureServices(IServiceCollection services)
        {
            // Load Configuration from App.config
            string apiUrl = ConfigurationManager.AppSettings["StudentMarkSheetUrl"]
                ?? throw new InvalidOperationException("Missing configuration: StudentMarkSheetUrl in App.config.");

            // Load Post Url Configuration from App.config
            string postApiUrl = ConfigurationManager.AppSettings["PostStudentMarkSheetUrl"]
                ?? throw new InvalidOperationException("Missing configuration: PostStudentMarkSheetUrl in App.config.");


            string connectionString = ConfigurationManager.ConnectionStrings["StudentMarksDB"]?.ConnectionString
                ?? throw new InvalidOperationException("Missing connection string: StudentMarksDB in App.config.");

            // Register Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            // Register Configuration Class(AppSettings)
            services.Configure<AppSettings>(options =>
            {
                options.StudentMarkSheetUrl = apiUrl;
                options.PostStudentMarkSheetUrl = postApiUrl;
            });

            // Register HttpClient for API Service
            services.AddHttpClient<IStudentMarksheetApiService, StudentMarksheetApiService>();

            // Register Window Service
            services.AddSingleton<IWindowService, WindowService>();

            // Register DbContext for SQL Server
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(connectionString));

            // Register Data Access Layer with Dependency Injection
            services.AddSingleton<IStudentMarkSheetDataAccess>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<StudentMarkSheetDataAccess>>();
                var dbContext = provider.GetRequiredService<AppDBContext>(); // Inject AppDBContext
                var apiService = provider.GetRequiredService<IStudentMarksheetApiService>(); // Inject API Service

                return new StudentMarkSheetDataAccess(dbContext, apiService, logger);
            });


            // Register ViewModels and Views
            services.AddTransient<StudentMarkSheetViewModel>();
            services.AddTransient<StudentMarksheetView>();
        }

        /// <summary>
        /// Configures Global Exception Handling for UI and Non-UI Threads
        /// </summary>
        private void ConfigureGlobalExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                try
                {
                    var logger = _serviceProvider?.GetRequiredService<ILogger<App>>();
                    if (e.ExceptionObject is Exception ex)
                    {
                        logger?.LogCritical(ex, "A critical non-UI thread exception occurred.");
                        Log.Fatal(ex, "A critical error occurred.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Failed to log unhandled exception.");
                }

                MessageBox.Show("A critical error occurred. Please check the logs.", "Critical Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1); // Exit application on fatal error
            };

            DispatcherUnhandledException += (sender, e) =>
            {
                try
                {
                    var logger = _serviceProvider?.GetRequiredService<ILogger<App>>();
                    logger?.LogError(e.Exception, "An unhandled UI exception occurred.");
                    Log.Error(e.Exception, "Unhandled UI Exception");
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Failed to log UI exception.");
                }

                MessageBox.Show("An unexpected error occurred. Please check the log file.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            };
        }

        /// <summary>
        /// Configures Serilog for Logging
        /// </summary>
        private void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/studentmarksheet_log.txt", rollingInterval: RollingInterval.Day) // File Logging
                .CreateLogger();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Environment.Exit(0); // Ensures all processes exit
            Log.CloseAndFlush(); // Ensure all logs are flushed
        }
    }
}
