using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    internal class Client
    {
        public string FullName { get; set; } // ФИО КЛИЕНТА , позволяем получить и установить значение
        public int PassportNumber { get; set; } // НОМЕР ПАСПОРТА
        public DateTime DateOfBirth { get; set; } // Дата РОЖДЕНИЯ

        public Client(string fullName, int passportNumber, DateTime dateOfBirth)
        {
            FullName = fullName;
            PassportNumber = passportNumber;
            DateOfBirth = dateOfBirth;
        }

        // ВОЗВРАЩАЕМ СТРОКУ, СОДЕРЖАЩИЮ ИНФО О КЛИЕНТЕ
        public string GetClientInfo()
        {
            return $"{FullName}, {PassportNumber}, {DateOfBirth:dd.MM.yyyy}";
        }
    }
}