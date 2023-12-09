using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace module16_pr
{
    using System;
    using System.IO;

    class Program
    {
        static string directoryToWatch;
        static string logFilePath;

        static void Main()
        {
            Console.WriteLine("Добро пожаловать в приложение для отслеживания изменений в файлах!");

            // Настройки слежения
            Console.Write("Введите путь к отслеживаемой директории: ");
            directoryToWatch = Console.ReadLine();

            Console.Write("Введите путь к лог-файлу: ");
            logFilePath = Console.ReadLine();

            try
            {
                // Статус существования директории
                if (!Directory.Exists(directoryToWatch))
                {
                    Console.WriteLine("Директория не существует. Пожалуйста, проверьте путь и перезапустите программу.");
                    return;
                }

                // Инициализация и запуск слежения
                using (FileSystemWatcher watcher = new FileSystemWatcher(directoryToWatch))
                {
                    // Указываем типы изменений, которые будем отслеживать
                    watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;

                    // Подписываемся на события
                    watcher.Created += OnChanged;
                    watcher.Deleted += OnChanged;
                    watcher.Renamed += OnRenamed;

                    // Запускаем слежение
                    watcher.EnableRaisingEvents = true;

                    Console.WriteLine($"Отслеживание изменений в директории {directoryToWatch}. Нажмите Enter для завершения.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void OnChanged(object sender, FileSystemEventArgs e)
        {
            string logMessage = $"{DateTime.Now} - {e.ChangeType}: {e.FullPath}";

            LogToFile(logMessage);
        }

        static void OnRenamed(object sender, RenamedEventArgs e)
        {
            string logMessage = $"{DateTime.Now} - {e.ChangeType}: {e.OldFullPath} переименовано в {e.FullPath}";

            LogToFile(logMessage);
        }

        static void LogToFile(string message)
        {
            try
            {

                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(message);
                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи в лог-файл: {ex.Message}");
            }
        }
    }

}
