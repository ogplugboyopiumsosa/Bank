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
        public int AccountNumber { get; set; }
        public DateTime OpeningDate { get; set; }
        public Client Owner { get; set; }
        public decimal Balance { get; private set; }
        public int DepositTermDays { get; private set; }
        public string Status { get; private set; } // Открыт, Закрыт, Банкрот

        public BankAccount(int accountNumber, Client owner, decimal initialBalance, int depositTermDays)
        {
            AccountNumber = accountNumber;
            Owner = owner;
            Balance = initialBalance;
            DepositTermDays = depositTermDays;
            OpeningDate = DateTime.Now;
            Status = "Открыт";
        }

        //Пополнение
        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Сумма должна быть положительной.");
            Balance += amount;
            DepositEndDate();
        }

        // Снятие со счета
        public void Withdraw(decimal amount)
        {
            if (amount > Balance) throw new InvalidOperationException("Недостаточно средств.");
            if (Status != "Открыт") throw new InvalidOperationException("Счет не открыт для операций");
            Balance -= amount;
            DepositEndDate();
        }

        private DateTime DepositEndDate()
        {
            return OpeningDate.AddDays(DepositTermDays);
        }

        public void UpdateStatus()
        {
            if (Balance < 0)
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

        // Метод перевода на другой счет
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
    }
}