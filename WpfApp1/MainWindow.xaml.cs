using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Classes;
using System.Text.RegularExpressions;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<BankAccount> accounts; // Список всех созданых счетов
        private BankAccount actualAccount; // Текущий выбранный счет

        public MainWindow()
        {
            InitializeComponent();
            accounts = new List<BankAccount>(); // Инициализация списка счетов

            // dpdateOfBirth.MaxDate = DateTime.Now;
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на ввод только чисел
            if (!int.TryParse(tbaccountNumber.Text, out int accountNumber))
            {
                MessageBox.Show("Введите корректный номер счета (только числа).");
                return;
            }

            // Проверка на уникальность номера счета
            if (accounts.Any(a => a.AccountNumber == accountNumber))
            {
                MessageBox.Show("Счет с таким номером уже существует.");
                return;
            }

            string ownerName = tbownerName.Text;
            // Проверка, что имя состоит только из букв (и пробела)
            if (!Regex.IsMatch(ownerName, @"^[A-Za-zА-Яа-яЁё\s]+$"))
            {
                MessageBox.Show("Имя владельца должно содержать только буквы и пробелы.");
                return;
            }
            
            // Проверка на ввод только чисел и мак. кол-во 10
            if (tbpassportNumber.Text.Length != 10 || !int.TryParse(tbpassportNumber.Text, out int passportNumber))
            {
                MessageBox.Show("Введите корректный номер паспорта (10 чисел).");
                return;
            }

            DateTime dateOfBirth = dpdateOfBirth.SelectedDate ?? DateTime.Now;
            // Проверка, чтобы дата рождения не была в будущем
            if (dateOfBirth > DateTime.Now)
            {
                MessageBox.Show("Дата рождения не может быть в будущем.");
                return;
            }

            // Проверка на ввод только чисел
            if (!decimal.TryParse(tbbalance.Text, out decimal balance))
            {
                MessageBox.Show("Введите корректный баланс.");
                return;
            }

            // Проверка на ввод только чисел
            if (!int.TryParse(tbdepositTermDays.Text, out int depositTermDays))
            {
                MessageBox.Show("Введите корректный срок депозита (в днях).");
                return;
            }

            Client client = new Client(ownerName, passportNumber, dateOfBirth);

            var account = new BankAccount(accountNumber, client, balance, depositTermDays);

            accounts.Add(account);

            UpdateClientList();

            tblistInfo.Text = account.GetAccountInfo();
        }

        // Добавление номеров счетов
        private void UpdateClientList()
        {
            cblistClients.Items.Clear();  // Очистка списка клиентов

            foreach (var account in accounts)
            {
                cblistClients.Items.Add(account.AccountNumber);  
            }
        }

        // Пополнение
        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            if (actualAccount == null)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            decimal amount = decimal.Parse(tbbalance.Text);  
            actualAccount.Deposit(amount);  

            tblistInfo.Text = actualAccount.GetAccountInfo();  
        }

        // Снятие
        private void Withdraw_Click(object sender, RoutedEventArgs e)
        {
            if (actualAccount == null)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            if (decimal.TryParse(tbbalance.Text, out decimal amount))
            {
                if (actualAccount.Withdraw(amount))  // Проверяем, можно ли снять деньги
                {
                    tblistInfo.Text = actualAccount.GetAccountInfo();
                }
                else
                {
                    MessageBox.Show("Недостаточно средств.");
                }
            }
            else
            {
                MessageBox.Show("Введите корректную сумму для снятия.");
            }

            
        }

        // Перевод
        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            if (actualAccount == null)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            // Проверка суммы перевода
            if (!decimal.TryParse(tbbalance.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму для перевода.");
                return;
            }

            // Проверка номера счета
            if (!int.TryParse(tbaccountNumber.Text, out int targetAccountNumber))
            {
                MessageBox.Show("Введите корректный номер целевого счёта.");
                return;
            }

            // Проверка счета в списке
            var targetAccount = accounts.FirstOrDefault(acc => acc.AccountNumber == targetAccountNumber);
            if (targetAccount == null)
            {
                MessageBox.Show("Целевой счёт не найден.");
                return;
            }

            // Сам перевод
            if (actualAccount.Balance >= amount)
            {
                actualAccount.Transfer(targetAccount, amount);
                tblistInfo.Text = actualAccount.GetAccountInfo();
                MessageBox.Show($"Успешно переведено {amount} руб. на счёт {targetAccountNumber}.");
            }
            else
            {
                MessageBox.Show("Ошибка перевода: недостаточно средств.");
            }
        }

        // Проверка выбран ли клиент в выпадающем списке
        private void ListClient_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cblistClients.SelectedItem == null)
            {
                actualAccount = null;
                tblistInfo.Text = "Клиент не выбран.";
                return;
            }

            int selectedAccountNumber = (int)cblistClients.SelectedItem; // Получаем выбранный номер счета
            actualAccount = accounts.FirstOrDefault(a => a.AccountNumber == selectedAccountNumber);

            if (actualAccount != null)
            {
                tblistInfo.Text = actualAccount.GetAccountInfo();
            }
            else
            {
                MessageBox.Show("Ошибка: не удалось найти выбранный счет.");
            }

        }
    }
}
