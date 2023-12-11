using B1WPFTestTask.DAL.Models;
using B1WPFTestTask.DAL.Repositories.Base;
using B1WPFTestTask.Models;
using B1WPFTestTask.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace B1WPFTestTask.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IRepository<FileInformation> _fileInfoRepository;

        public FileService(IRepository<FileInformation> fileInfoRepository)
        {
            _fileInfoRepository = fileInfoRepository;
        }

        /// <summary>
        /// Получает коллекцию ImportedFile из репозитория файлов.
        /// </summary>
        /// <returns>ObservableCollection файлов.</returns>
        public async Task<ObservableCollection<ImportedFile>> GetFilesAsync()
        {
            // Получаем все файлы из репозитория
            var files = await _fileInfoRepository.GetAllAsync();

            // Преобразуем в ImportedFile и создаем коллекцию
            var importedFiles = files.Select(f => new ImportedFile
            {
                FileName = f.FileName,
                Id = f.Id,
            });

            // Создаем и возвращаем ObservableCollection
            var observableCollection = new ObservableCollection<ImportedFile>(importedFiles);
            return observableCollection;
        }
    }
}
