using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZenTotem
{
    public class Employee
    {
        public int Id { get; set; }         // Идентификатор сотрудника
        public string FirstName { get; set; }   // Имя сотрудника
        public string LastName { get; set; }    // Фамилия сотрудника
        public decimal SalaryPerHour { get; set; }  // Зарплата сотрудника за час работы
    }

    public class Program
    {
       public static string filePath = "employees.json";  // Путь к файлу с данными сотрудников
        public static List<Employee> employees;  // Список сотрудников

        public static void Main(string[] args)
        {
            employees = LoadEmployees();   // Загрузка данных о сотрудниках из файла

            if (args.Length == 0)
            {
                Console.WriteLine("Никаких аргументов представлено не было.");  // Если не были указаны аргументы командной строки
                Console.WriteLine("В данной программе доступны следующие аргументы для командной строки:\n\r\n\r-add - добавляет нового сотрудника в список. Требует указания следующих аргументов:\r\n\r\nfirstName - имя сотрудника;\r\nlastName - фамилия сотрудника;\r\nsalary - заработная плата сотрудника.\r\n-update - обновляет информацию о существующем сотруднике. Требует указания следующих аргументов:\r\n\r\nid - идентификатор сотрудника, которого нужно обновить;\r\nодин или несколько из следующих аргументов, указывающих поля для обновления:\r\nfirstName - новое имя сотрудника;\r\nlastName - новая фамилия сотрудника;\r\nsalary - новая заработная плата сотрудника.\r\n-get - получает информацию о существующем сотруднике. Требует указания следующего аргумента:\r\n\r\nid - идентификатор сотрудника, о котором нужно получить информацию.\r\n-delete - удаляет существующего сотрудника из списка. Требует указания следующего аргумента:\r\n\r\nid - идентификатор сотрудника, которого нужно удалить.\r\n-getall - выводит информацию о всех сотрудниках.\r\n\r\nПримеры использования аргументов:\r\n\r\nДобавление нового сотрудника:\r\nZenTotem.exe -add firstName:John lastName:Doe salary:1000\r\n\r\nОбновление имени и заработной платы существующего сотрудника:\r\nZenTotem.exe -update id:1 firstName:John lastName:Smith salary:1200\r\n\r\nПолучение информации о сотруднике с определенным идентификатором:\r\nZenTotem.exe -get id:1\r\n\r\nУдаление сотрудника с определенным идентификатором:\r\nZenTotem.exe -delete id:1\r\n\r\nВывод информации о всех сотрудниках:\r\nZenTotem.exe -getall");
                return;
            }
            Console.WriteLine(args[0]);
            string command = args[0].ToLower();  // Получение команды из первого аргумента

            switch (command)
            {
                case "-add":
                    AddEmployee(args);   // Добавление сотрудника
                    break;
                case "-update":
                    UpdateEmployee(args);  // Обновление данных сотрудника
                    break;
                case "-get":
                    GetEmployee(args);   // Получение данных сотрудника
                    break;
                case "-delete":
                    DeleteEmployee(args);   // Удаление сотрудника
                    break;
                case "-getall":
                    GetAllEmployees();   // Вывод всех сотрудников
                    break;
                default:
                    Console.WriteLine("Недопустимая команда.");   // Недопустимая команда
                    break;
            }

            SaveEmployees();   // Сохранение данных о сотрудниках в файл
        }

        public static List<Employee> LoadEmployees()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);  // Чтение данных из файла в формате JSON
                return JsonConvert.DeserializeObject<List<Employee>>(json);  // Десериализация JSON в список сотрудников
            }
            else
            {
                return new List<Employee>();   // Если файл не существует, создаем пустой список сотрудников
            }
        }

        public static void SaveEmployees()
        {
            string json = JsonConvert.SerializeObject(employees, Newtonsoft.Json.Formatting.Indented);  // Сериализация списка сотрудников в формат JSON
            File.WriteAllText(filePath, json);  // Запись данных в файл
        }

        public static void AddEmployee(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Недопустимое количество аргументов для добавления сотрудника.");  // Неправильное количество аргументов для добавления сотрудника
                return;
            }
            Console.WriteLine(args[1]+"\n"+ args[2]+"\n"+ args[3]);


            string firstName = GetValueFromArgument(args[1]);   // Получение имени сотрудника из аргумента
            string lastName = GetValueFromArgument(args[2]);   // Получение фамилии сотрудника из аргумента
            decimal salary = Convert.ToDecimal(GetValueFromArgument(args[3]));   // Получение заработной платы сотрудника из аргумента

            int maxId = employees.Count > 0 ? employees.Max(emp => emp.Id) : 0;   // Поиск максимального идентификатора среди существующих сотрудников
            int newId = maxId + 1;   // Генерация нового идентификатора для нового сотрудника
            Employee newEmployee = new Employee
            {
                Id = newId,
                FirstName = firstName,
                LastName = lastName,
                SalaryPerHour = salary
            };

            employees.Add(newEmployee);   // Добавление нового сотрудника в список

            Console.WriteLine("Сотрудник успешно добавлен.");
        }


        public static void UpdateEmployee(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Недопустимое количество аргументов для обновления сотрудника.");  // Неправильное количество аргументов для обновления сотрудника
                return;
            }
            Console.WriteLine(args[1]);

            int id = Convert.ToInt32(GetValueFromArgument(args[1]));  // Получение идентификатора сотрудника из аргумента
            Employee employee = employees.Find(emp => emp.Id == id);  // Поиск сотрудника по идентификатору

            if (employee == null)
            {
                Console.WriteLine("Сотрудник не найден.");  // Сотрудник не найден
                return;
            }

            for (int i = 2; i < args.Length; i++)
            {
                Console.WriteLine(args[i]);
                string[] keyValue = args[i].Split(':');
                Console.WriteLine(keyValue[1]);
                string key = keyValue[0].ToLower();
                string value = keyValue[1];




                switch (key)
                {
                    case "firstname":
                        employee.FirstName = value;   // Обновление имени сотрудника
                        break;
                    case "lastname":
                        employee.LastName = value;   // Обновление фамилии сотрудника
                        break;
                    case "salary":
                        employee.SalaryPerHour = Convert.ToDecimal(value);   // Обновление заработной платы сотрудника
                        break;
                    default:
                        Console.WriteLine($"Недопустимое поле: {key}");   // Недопустимое поле для обновления
                        break;
                }
            }

            Console.WriteLine("Сотрудник успешно обновлен.");
        }

        public static void GetEmployee(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Недопустимое количество аргументов для получения сотрудника.");  // Неправильное количество аргументов для получения сотрудника
                return;
            }

            int id = Convert.ToInt32(GetValueFromArgument(args[1]));  // Получение идентификатора сотрудника из аргумента
            Employee employee = employees.Find(emp => emp.Id == id);  // Поиск сотрудника по идентификатору

            if (employee == null)
            {
                Console.WriteLine("Сотрудник не найден.");  // Сотрудник не найден
                return;
            }

            Console.WriteLine($"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}");
        }

        public static void DeleteEmployee(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Недопустимое количество аргументов для удаления сотрудника.");  // Неправильное количество аргументов для удаления сотрудника
                return;
            }

            int id = Convert.ToInt32(GetValueFromArgument(args[1]));  // Получение идентификатора сотрудника из аргумента
            Employee employee = employees.Find(emp => emp.Id == id);  // Поиск сотрудника по идентификатору

            if (employee == null)
            {
                Console.WriteLine("Сотрудник не найден.");  // Сотрудник не найден
                return;
            }

            employees.Remove(employee);  // Удаление сотрудника из списка

            Console.WriteLine("Сотрудник успешно удален.");
        }

        public static void GetAllEmployees()
        {
            foreach (Employee employee in employees)
            {
                Console.WriteLine($"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}");
            }
        }

        public static string GetValueFromArgument(string argument)
        {
            return argument.Split(':')[1];   // Извлечение значения из аргумента в формате "ключ:значение"
        }
    }
}
