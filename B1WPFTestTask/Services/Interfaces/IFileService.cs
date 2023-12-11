using B1WPFTestTask.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace B1WPFTestTask.Services.Interfaces;

public interface IFileService
{
    Task<ObservableCollection<ImportedFile>> GetFilesAsync();
}
