using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExpenseTracker
{
    class Expense
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public Expense(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Name}; {Amount} руб.";
        }
    }

    class Program
    {
        static List<Expense> expenses = new List<Expense>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== УЧЕТ РАСХОДОВ ===");
            int operationsCount = GetOperationsCount();
            InputExpenses(operationsCount);
            ShowMainMenu();
        }

        static int GetOperationsCount()
        {
            int count;
            while (true)
            {
                Console.Write("Введите количество операций (2-40): ");
                if (int.TryParse(Console.ReadLine(), out count) && count >= 2 && count <= 40)
                {
                    return count;
                }
                Console.WriteLine("Ошибка! Введите число от 2 до 40.");
            }
        }

        static void InputExpenses(int count)
        {
            Console.WriteLine("\nВведите траты в формате: Название; Сумма");
            Console.WriteLine("Пример: Влажные салфетки \"Лента\"; 235");

            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    Console.Write($"Операция {i + 1}: ");
                    string input = Console.ReadLine();

                    if (TryParseExpense(input, out Expense expense))
                    {
                        expenses.Add(expense);
                        break;
                    }
                    Console.WriteLine("Ошибка формата! Используйте: Название; Сумма");
                }
            }

            Console.WriteLine("\nВсе операции успешно добавлены!");
        }

        static bool TryParseExpense(string input, out Expense expense)
        {
            expense = null;
            if (string.IsNullOrWhiteSpace(input))
                return false;

            int separatorIndex = input.LastIndexOf(';');
            if (separatorIndex == -1)
                return false;

            string name = input.Substring(0, separatorIndex).Trim();
            string amountStr = input.Substring(separatorIndex + 1).Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(amountStr))
                return false;

            if (decimal.TryParse(amountStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) && amount > 0)
            {
                expense = new Expense(name, amount);
                return true;
            }

            return false;
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
                Console.WriteLine("1. Вывод данных");
                Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
                Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
                Console.WriteLine("4. Конвертация валюты");
                Console.WriteLine("5. Поиск по названию");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт меню: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowExpenses();
                        break;
                    case "2":
                        ShowStatistics();
                        break;
                    case "3":
                        BubbleSortByPrice();
                        break;
                    case "4":
                        CurrencyConversion();
                        break;
                    case "5":
                        SearchByName();
                        break;
                    case "0":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        static void ShowExpenses()
        {
            Console.WriteLine("\n=== ВСЕ ТРАТЫ ===");
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных о тратах.");
                return;
            }

            for (int i = 0; i < expenses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {expenses[i]}");
            }

            Console.WriteLine($"\nВсего операций: {expenses.Count}");
        }

        static void ShowStatistics()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных для статистики.");
                return;
            }

            decimal total = expenses.Sum(e => e.Amount);
            decimal average = expenses.Average(e => e.Amount);
            decimal max = expenses.Max(e => e.Amount);
            decimal min = expenses.Min(e => e.Amount);

            Console.WriteLine("\n=== СТАТИСТИКА ===");
            Console.WriteLine($"Общая сумма: {total:F2} руб.");
            Console.WriteLine($"Средняя трата: {average:F2} руб.");
            Console.WriteLine($"Максимальная трата: {max:F2} руб.");
            Console.WriteLine($"Минимальная трата: {min:F2} руб.");
            Console.WriteLine($"Количество операций: {expenses.Count}");
        }

        static void BubbleSortByPrice()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных для сортировки.");
                return;
            }

            List<Expense> sortedList = new List<Expense>(expenses);

            for (int i = 0; i < sortedList.Count - 1; i++)
            {
                for (int j = 0; j < sortedList.Count - i - 1; j++)
                {
                    if (sortedList[j].Amount > sortedList[j + 1].Amount)
                    {
                        Expense temp = sortedList[j];
                        sortedList[j] = sortedList[j + 1];
                        sortedList[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine("\n=== СОРТИРОВКА ПО ЦЕНЕ (ПО ВОЗРАСТАНИЮ) ===");
            for (int i = 0; i < sortedList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sortedList[i]}");
            }

            Console.Write("\nПрименить сортировку к основному списку? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                expenses = sortedList;
                Console.WriteLine("Сортировка применена!");
            }
        }
        static void CurrencyConversion()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных для конвертации.");
                return;
            }

            Console.WriteLine("\n=== КОНВЕРТАЦИЯ ВАЛЮТЫ ===");
            Console.WriteLine("Доступные валюты:");
            Console.WriteLine("1. Доллар США (USD)");
            Console.WriteLine("2. Евро (EUR)");
            Console.WriteLine("3. Фунт стерлингов (GBP)");
            Console.WriteLine("4. Произвольный курс");

            Console.Write("Выберите валюту или введите 0 для отмены: ");
            string choice = Console.ReadLine();

            decimal rate = 0;
            string currencyName = "";

            switch (choice)
            {
                case "1":
                    rate = GetExchangeRate("долларов");
                    currencyName = "USD";
                    break;
                case "2":
                    rate = GetExchangeRate("евро");
                    currencyName = "EUR";
                    break;
                case "3":
                    rate = GetExchangeRate("фунтов");
                    currencyName = "GBP";
                    break;
                case "4":
                    rate = GetCustomExchangeRate();
                    currencyName = "иностранной валюты";
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    return;
            }

            if (rate <= 0)
            {
                Console.WriteLine("Неверный курс!");
                return;
            }

            Console.WriteLine($"\n=== ТРАТЫ В {currencyName} (курс: {rate:F2}) ===");
            foreach (var expense in expenses)
            {
                decimal convertedAmount = expense.Amount / rate;
                Console.WriteLine($"{expense.Name}; {convertedAmount:F2} {currencyName}");
            }
        }

        static decimal GetExchangeRate(string currencyName)
        {
            Console.Write($"Введите курс рубля к {currencyName}: ");
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate) && rate > 0)
            {
                return rate;
            }
            return 0;
        }

        static decimal GetCustomExchangeRate()
        {
            Console.Write("Введите произвольный курс (рублей за 1 единицу валюты): ");
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate) && rate > 0)
            {
                return rate;
            }
            return 0;
        }

        static void SearchByName()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных для поиска.");
                return;
            }

            Console.Write("\nВведите название для поиска: ");
            string searchTerm = Console.ReadLine().ToLower();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("Пустой поисковый запрос!");
                return;
            }

            var results = expenses.Where(e => e.Name.ToLower().Contains(searchTerm)).ToList();

            Console.WriteLine($"\n=== РЕЗУЛЬТАТЫ ПОИСКА: '{searchTerm}' ===");

            if (results.Count == 0)
            {
                Console.WriteLine("Ничего не найдено.");
                return;
            }

            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {results[i]}");
            }

            Console.WriteLine($"Найдено: {results.Count} операций");
        }
    }
}
