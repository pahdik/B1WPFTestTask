using B1WPFTestTask.Models;
using B1WPFTestTask.Services.Interfaces;
using B1WPFTestTask.Utils;
using B1WPFTestTask.Views;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace B1WPFTestTask.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IExcelDataImporterService _excelDataImporterService;
        private readonly IFileService _fileService;
        private readonly IDataService _dataService;
        private string _selectedExcelFilePath;
        private ObservableCollection<ImportedFile> _importedFiles;
        private ImportedFile _selectedFileName;
        private ObservableCollection<FinancialData> _dataItems;

        // Коллекция для отображения данных в DataGrid
        public ObservableCollection<FinancialData> DataItems
        {
            get { return _dataItems; }
            set
            {
                _dataItems = value;
                OnPropertyChanged();
            }
        }

        // Коллекция для отображения списка импортированных файлов
        public ObservableCollection<ImportedFile> ImportedFiles
        {
            get { return _importedFiles; }
            set
            {
                _importedFiles = value;
                OnPropertyChanged();
            }
        }

        // Выбранный файл из списка
        public ImportedFile SelectedFileName
        {
            get { return _selectedFileName; }
            set
            {
                _selectedFileName = value;
                OnPropertyChanged();
            }
        }

        // Путь к выбранному Excel-файлу
        public string SelectedExcelFilePath
        {
            get { return _selectedExcelFilePath; }
            set
            {
                _selectedExcelFilePath = value;
                OnPropertyChanged();
            }
        }

        // Команда для открытия диалогового окна выбора Excel-файла
        private RelayCommand _openFileDialogCommand;
        public ICommand OpenFileDialogCommand
        {
            get
            {
                return _openFileDialogCommand ?? (_openFileDialogCommand = new RelayCommand(param => OpenExcelFileDialog()));
            }
        }

        // Команда для импорта данных из Excel-файла в базу данных
        private RelayCommand _importDataCommand;
        public ICommand ImportDataCommand
        {
            get
            {
                return _importDataCommand ?? (_importDataCommand = new RelayCommand(async param => await ImportDataClicked()));
            }
        }

        // Команда для отображения данных в таблице
        private RelayCommand _showTableCommand;
        public ICommand ShowTableCommand
        {
            get
            {
                return _showTableCommand ?? (_showTableCommand = new RelayCommand(async param => await ShowTableClicked()));
            }
        }

        public MainViewModel(
            IExcelDataImporterService excelDataImporterService,
            IFileService fileService,
            IDataService dataService)
        {
            _excelDataImporterService = excelDataImporterService;
            _fileService = fileService;
            _dataService = dataService;

            // Инициализация списка импортированных файлов
            InitializeImportedFilesAsync();
        }

        // Асинхронная инициализация списка импортированных файлов
        private async Task InitializeImportedFilesAsync()
        {
            ImportedFiles = await _fileService.GetFilesAsync();
        }

        // Метод для открытия диалогового окна выбора Excel-файла
        private void OpenExcelFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Файлы Excel|*.xls;*.xlsx",
                Title = "Выберите Excel-файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedExcelFilePath = openFileDialog.FileName;
            }
        }

        // Асинхронный метод для обработки клика по кнопке "Импорт данных"
        private async Task ImportDataClicked()
        {
            await _excelDataImporterService.ImportDataToDatabase(SelectedExcelFilePath);
            await InitializeImportedFilesAsync();
        }

        // Асинхронный метод для обработки клика по кнопке "Показать таблицу"
        private async Task ShowTableClicked()
        {
            DataItems = await _dataService.GetFinancialDataAsync(SelectedFileName.Id);

            // Создаем и показываем окно с данными
            var dataWindow = new DataWindow();
            dataWindow.dataGrid.ItemsSource = DataItems;
            dataWindow.Show();
        }

        // Событие изменения свойства
        public event PropertyChangedEventHandler PropertyChanged;

        // Метод для вызова события изменения свойства
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
