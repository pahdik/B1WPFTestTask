using B1WPFTestTask.DAL.Data;
using B1WPFTestTask.DAL.Entities;
using B1WPFTestTask.DAL.Models;
using B1WPFTestTask.DAL.Repositories.Base;
using B1WPFTestTask.DAL.Repositories.Implementations;
using B1WPFTestTask.Services.Implementations;
using B1WPFTestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace B1WPFTestTask
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQLServer"));
            });
            serviceCollection.AddTransient<IRepository<Bank>, Repository<Bank>>();
            serviceCollection.AddTransient<IRepository<AccountClass>, Repository<AccountClass>>();
            serviceCollection.AddTransient<IRepository<FileInformation>, Repository<FileInformation>>();
            serviceCollection.AddTransient<IRepository<Balance>, Repository<Balance>>();
            serviceCollection.AddTransient<IRepository<AccountGroup>, Repository<AccountGroup>>();
            serviceCollection.AddTransient<IExcelDataImporterService, ExcelDataImporterService>();
            serviceCollection.AddTransient<IFileService, FileService>();
            serviceCollection.AddTransient<IDataService, DataService>();
            serviceCollection.AddTransient(typeof(MainWindow));
        }
    }
}
