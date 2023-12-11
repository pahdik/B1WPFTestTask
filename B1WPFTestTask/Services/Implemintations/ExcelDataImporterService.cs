using B1WPFTestTask.DAL.Entities;
using B1WPFTestTask.DAL.Models;
using B1WPFTestTask.DAL.Repositories.Base;
using B1WPFTestTask.Services.Interfaces;
using OfficeOpenXml;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace B1WPFTestTask.Services.Implementations
{
    public class ExcelDataImporterService : IExcelDataImporterService
    {
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<FileInformation> _fileInfoRepository;
        private readonly IRepository<Balance> _balanceRepository;
        private readonly IRepository<AccountClass> _accountClassRepository;
        private readonly IRepository<AccountGroup> _accountGroupRepository;

        public ExcelDataImporterService(
            IRepository<Bank> bankRepository,
            IRepository<FileInformation> fileInfoRepository,
            IRepository<Balance> balanceRepository,
            IRepository<AccountClass> accountClassRepository,
            IRepository<AccountGroup> accountGroupRepository)
        {
            _bankRepository = bankRepository;
            _fileInfoRepository = fileInfoRepository;
            _balanceRepository = balanceRepository;
            _accountClassRepository = accountClassRepository;
            _accountGroupRepository = accountGroupRepository;
        }

        /// <summary>
        /// Импортирует данные из Excel-файла в базу данных.
        /// </summary>
        /// <param name="filePath">Путь к Excel-файлу.</param>
        public async Task ImportDataToDatabase(string filePath)
        {
            try
            {
                // Создаем информацию о файле
                var fileInformation = new FileInformation
                {
                    FileName = Path.GetFileName(filePath)
                };
                var currentFile = await _fileInfoRepository.CreateAsync(fileInformation);

                // Открываем Excel-пакет
                using var package = new ExcelPackage(new FileInfo(filePath));

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;

                AccountClass currentClass = null;

                // Итерируем по строкам Excel-таблицы, начиная с 9 строки
                for (int row = 9; row <= rowCount; row++)
                {
                    var firstCell = worksheet.Cells[row, 1].Text.Trim();

                    if (firstCell.Contains("КЛАСС"))
                    {
                        // Если встречается "КЛАСС", создаем или получаем объект класса
                        var accountClass = new AccountClass
                        {
                            Name = firstCell
                        };
                        currentClass = await GetOrCreateAccountClassAsync(accountClass);
                    }
                    else if (int.TryParse(firstCell, out int accountNumber))
                    {
                        if (accountNumber >= 1000 && accountNumber < 10000)
                        {
                            // Если встречается номер счета, создаем или получаем объект группы и добавляем баланс
                            var accountGroup = new AccountGroup
                            {
                                GroupNumber = accountNumber / 100
                            };
                            var currentGroup = await GetOrCreateAccountGroupAsync(accountGroup);

                            var newBalance = new Balance
                            {
                                AccountNumber = accountNumber,
                                AccountClassId = currentClass.Id,
                                AccountGroupId = currentGroup.Id,
                                FileInformationId = currentFile.Id,
                                IncomingSaldoActive = decimal.Parse(worksheet.Cells[row, 2].Text, NumberStyles.Number, CultureInfo.GetCultureInfo("ru-RU")),
                                IncomingSaldoPassive = decimal.Parse(worksheet.Cells[row, 3].Text, NumberStyles.Number, CultureInfo.GetCultureInfo("ru-RU")),
                                TurnoverDebit = decimal.Parse(worksheet.Cells[row, 4].Text, NumberStyles.Number, CultureInfo.GetCultureInfo("ru-RU")),
                                TurnoverCredit = decimal.Parse(worksheet.Cells[row, 5].Text, NumberStyles.Number, CultureInfo.GetCultureInfo("ru-RU")),
                            };

                            await _balanceRepository.CreateAsync(newBalance);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Выводим сообщение об ошибке в консоль
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        // Метод для получения или создания объекта AccountClass
        private async Task<AccountClass> GetOrCreateAccountClassAsync(AccountClass entity)
        {
            var accountClass = await _accountClassRepository.GetFirstOrDefaultAsync(
                predicate: p => p.Name == entity.Name);

            if (accountClass == null)
            {
                var createdEntity = await _accountClassRepository.CreateAsync(entity);
                return createdEntity;
            }

            return accountClass;
        }

        // Метод для получения или создания объекта AccountGroup
        private async Task<AccountGroup> GetOrCreateAccountGroupAsync(AccountGroup entity)
        {
            var accountGroup = await _accountGroupRepository.GetFirstOrDefaultAsync(
                predicate: p => p.GroupNumber == entity.GroupNumber);

            if (accountGroup == null)
            {
                var createdEntity = await _accountGroupRepository.CreateAsync(entity);
                return createdEntity;
            }

            return accountGroup;
        }
    }
}
