using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    internal class BankAccount
    {
        public int AccountNumber { get; set; } // НОМЕР СЧЕТА
        public DateTime OpeningDate { get; set; } // ДАТА ОТКРЫТИЯ СЧЕТА
        public Client Owner { get; set; } // ИНФО О КЛИЕНТЕ
        public decimal Balance { get; private set; } // БАЛАНС СЧЕТА
        public int DepositTermDays { get; private set; } // СРОК ВКЛАДА
        public string Status { get; private set; } // Открыт, Закрыт, Банкрот


        public BankAccount(int accountNumber, Client owner, decimal initialBalance, int depositTermDays)
        {
            AccountNumber = accountNumber;
            Owner = owner;
            Balance = initialBalance;
            DepositTermDays = depositTermDays;
            OpeningDate = DateTime.Now;
            Status = "Открыт";
            UpdateStatus();
        }

        // Пополнение счета
        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Сумма должна быть положительной.");
            Balance += amount;
            UpdateStatus();
        }

        // Снятие со счета
        public bool Withdraw(decimal amount)
        {
            if (amount > Balance) return false; // Проверка средст на счете
            if (Status != "Открыт") throw new InvalidOperationException("Счет не открыт для операций");
            Balance -= amount;
            UpdateStatus();
            DepositEndDate();
            return true;
        }
        
        // Вычисление даты окончание вклада
        private DateTime DepositEndDate()
        {
            return OpeningDate.AddDays(DepositTermDays);
        }

        // Изменение статуса вклада
        public void UpdateStatus()
        {
            if (Balance <= 0)
            {
                Status = "Банкрот";
            }
            else if (DateTime.Now > DepositEndDate())
            {
                Status = "Закрыт";
            }
            else
            {
                Status = "Открыт";
            }
            
        }

        // Перевод
        public void Transfer(BankAccount targetAccount, decimal amount)
        {
            if (amount > 0 && Balance >= amount)
            {
                this.Withdraw(amount);
                targetAccount.Deposit(amount);
                Console.WriteLine($"Переведено {amount} на счет {targetAccount.AccountNumber}");
            }
            else
            {
                Console.WriteLine("Недостаточно средств для перевода.");
            }
        }

        // Возвращает строку с полной информацией о счете
        public string GetAccountInfo()
        {
            UpdateStatus();
            return $"Номер счета: {AccountNumber}\n" +
                   $"Дата открытия: {OpeningDate:dd.MM.yyyy}\n" +
                   $"Владелец: {Owner.GetClientInfo()}\n" +
                   $"Баланс: {Balance} руб.\n" +
                   $"Статус: {Status}\n" +
                   $"Дата окончания вклада: {DepositEndDate():dd.MM.yyyy}";
        }

    }
}