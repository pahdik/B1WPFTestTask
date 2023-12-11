using B1WPFTestTask.DAL.Models;
using B1WPFTestTask.DAL.Repositories.Base;
using B1WPFTestTask.Models;
using B1WPFTestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace B1WPFTestTask.Services.Implementations
{
    public class DataService : IDataService
    {
        private readonly IRepository<Balance> _balanceRepository;

        public DataService(IRepository<Balance> balanceRepository)
        {
            _balanceRepository = balanceRepository;
        }

        public async Task<ObservableCollection<FinancialData>> GetFinancialDataAsync(int fileId)
        {
            // Получаем балансы из репозитория
            var balances = await _balanceRepository.GetAllAsync(
                predicate: b => b.FileInformationId == fileId,
                orderBy: b => b.OrderBy(b => b.AccountNumber),
                include: b => b.Include(b => b.AccountGroup).Include(b => b.AccountClass).Include(b => b.FileInformation));

            var financialList = new List<FinancialData>();
            var sum = new FinancialDataSum();
            int currentGroup = balances.FirstOrDefault()?.AccountGroup.GroupNumber ?? 0;
            int currentClass = balances.FirstOrDefault()?.AccountClassId ?? 0;
            financialList.Add(new FinancialData()
            {
                AccountNumber = balances[0].AccountClass.Name
            });

            foreach (var balance in balances)
            {
                bool isGroupChange = balance.AccountGroup.GroupNumber != currentGroup;
                bool isClassChange = balance.AccountClassId != currentClass;

                // Проверяем изменение группы и добавляем сумму предыдущей группы
                if (isGroupChange)
                {
                    AddGroupSumToList(currentGroup, sum, financialList);
                    currentGroup = balance.AccountGroup.GroupNumber;
                    sum = new FinancialDataSum();
                }

                // Проверяем изменение класса и добавляем новый класс
                if (isClassChange)
                {
                    AddClassToList(balance.AccountClass.Name, financialList);
                    currentClass = balance.AccountClass.Id;
                }

                var outcomingSaldoActive = balance.IncomingSaldoActive != 0 ? balance.IncomingSaldoActive + balance.TurnoverDebit - balance.TurnoverCredit : 0;
                var outcomingSaldoPassive = balance.IncomingSaldoPassive != 0 ? balance.IncomingSaldoPassive - balance.TurnoverDebit + balance.TurnoverCredit : 0;

                // Обновляем сумму и добавляем новый элемент
                sum.Add(
                    balance.IncomingSaldoActive,
                    balance.IncomingSaldoPassive,
                    balance.TurnoverDebit,
                    balance.TurnoverCredit,
                    outcomingSaldoActive,
                    outcomingSaldoPassive);

                AddNewItemToList(balance.AccountNumber.ToString(), balance, outcomingSaldoActive, outcomingSaldoPassive, financialList);
            }

            // Добавляем сумму для последней группы
            AddGroupSumToList(currentGroup, sum, financialList);

            // Создаем ObservableCollection и возвращаем результат
            var observableCollection = new ObservableCollection<FinancialData>(financialList);
            return observableCollection;
        }

        // Добавляет сумму группы в список
        private void AddGroupSumToList(int currentGroup, FinancialDataSum sum, List<FinancialData> financialList)
        {
            var newItemSum = new FinancialData()
            {
                AccountNumber = currentGroup.ToString(),
                IncomingSaldoActive = sum.IncomingSaldoActive.ToString(),
                IncomingSaldoPassive = sum.IncomingSaldoPassive.ToString(),
                TurnoverDebit = sum.TurnoverDebit.ToString(),
                TurnoverCredit = sum.TurnoverCredit.ToString(),
                OutcomingSaldoActive = sum.OutcomingSaldoActive.ToString(),
                OutcomingSaldoPassive = sum.OutcomingSaldoPassive.ToString(),
            };
            financialList.Add(newItemSum);
        }

        // Добавляет новый класс в список
        private void AddClassToList(string className, List<FinancialData> financialList)
        {
            var newClass = new FinancialData()
            {
                AccountNumber = className
            };
            financialList.Add(newClass);
        }

        // Добавляет новый элемент в список
        private void AddNewItemToList(string accountNumber, Balance balance, decimal outcomingSaldoActive, decimal outcomingSaldoPassive, List<FinancialData> financialList)
        {
            var newItem = new FinancialData()
            {
                AccountNumber = accountNumber,
                IncomingSaldoActive = balance.IncomingSaldoActive.ToString(),
                IncomingSaldoPassive = balance.IncomingSaldoPassive.ToString(),
                TurnoverDebit = balance.TurnoverDebit.ToString(),
                TurnoverCredit = balance.TurnoverCredit.ToString(),
                OutcomingSaldoActive = outcomingSaldoActive.ToString(),
                OutcomingSaldoPassive = outcomingSaldoPassive.ToString()
            };
            financialList.Add(newItem);
        }
    }
}
