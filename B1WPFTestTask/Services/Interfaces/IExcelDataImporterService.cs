using System.Threading.Tasks;

namespace B1WPFTestTask.Services.Interfaces;

public interface IExcelDataImporterService
{
    public Task ImportDataToDatabase(string filePath);
}
