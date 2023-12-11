using B1WPFTestTask.Services.Interfaces;
using B1WPFTestTask.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace B1WPFTestTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            DataContext = new MainViewModel(
                serviceProvider.GetRequiredService<IExcelDataImporterService>(),
                serviceProvider.GetRequiredService<IFileService>(),
                serviceProvider.GetRequiredService<IDataService>());
        }
    }
}
