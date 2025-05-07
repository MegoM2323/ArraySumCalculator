/*
* Дисциплина: "Программирование на C#"
* Группа: БПИ-245
* Студент: Панин Михаил Павлович
* Дата: 04.10.2024
* Вариант: 4
* Задача: (Краткое описание) Программа получает из input.txt данные для двух массивов. По данным массивов одинаковой длины требуется вычислить значение S.
* 𝑆 = ∑ (𝑘=0, до N - 1) 2 / (𝑋𝑘 + 𝑌𝑘)
* В output.txt программа сохраняет значение 𝑆.
* 
* Коментарий: Ошибки которые в основной программе, которые прерывают ее выполнение в этом решении работают иначе.
* Поскольку тестов несколько если такая ошибка возникает -> она записывается как результат работы этого теста в output-X.txt (X - номер запуска программы)
* + Выведен результат обработки каждого теста на консоль (хорошо завершился + результат /или/ с чем связана ошибка) 
*/
using System.Text;

namespace HSE_p1_additional_task
{
    /// <summary>
    /// Класс для чтения/записи и работы с файлами.
    /// </summary>
    internal static class Text_Interactions
    {
        
        private static string path_input = $"../../../../input.txt"; // Путь до input.txt. По условию путь не должен изменяться.
        public static string path_output = $"../../../../"; // Путь до output.txt без названия файла сохранения (т.к. он меняется).


        /// <summary>
        /// Метод считывает данные из файла.
        /// </summary>
        /// <param name="path">Путь до читаемого файла, по умолчанию до input.txt.</param>
        /// <returns>Массив прочитанных строк.</returns>
        /// <exception cref="FileNotFoundException">При отсутствии файла по пути.</exception>
        public static string[] ReadFile(string? path = null)
        {
            if (path is null) // Если путь не указан -> путь до input.txt
            {
                path = path_input;
            }
            if (File.Exists(path))
            {
                string[] readText = File.ReadAllLines(path, Encoding.UTF8);
                return readText;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        /// <summary>
        /// Метод создающий 2 очищенных массива из 2-ух строк.
        /// </summary>
        /// <param name="readText">Массив из 2 строк с значениниями для формирования 2 массивов.</param>
        /// <returns>2 очищенных массива.</returns>
        /// <exception cref="ArgumentException">Исключение при пустом (входном) массиве формирования.</exception>
        public static (int[], int[]) Parser(string[] readText)
        {
            if (readText.Length == 0)
            {
                throw new ArgumentException("Файл пуст");
            }
            // Массивы неочищенных данных.
            string[] line_arr_1 = readText[0].Split();
            string[] line_arr_2 = readText[1].Split();

            // Строка для добавления очищенных данных, на основе которых будут сформированны 2 массива целых чисел.
            string res_line1 = "";
            string res_line2 = "";

            // Выявление корректных данных 1 строки.
            for (int i = 0; i < line_arr_1.Length; i++)
            {
                try
                {
                    res_line1 += int.Parse(line_arr_1[i]).ToString() + ' ';
                }
                // Некорректные данные просто пропускаем (не выводя сообщений) по условию моего варианта (завершать программу было бы не логично).
                catch (ArgumentNullException) { }
                catch (FormatException) { }
                catch (OverflowException) { }

            }

            // Выявление корректных данных 2 строки.
            for (int i = 0; i < line_arr_2.Length; i++)
            {
                try
                {
                    res_line2 += int.Parse(line_arr_2[i]).ToString() + ' ';
                }
                // Некорректные данные просто пропускаем (не выводя сообщений) по условию моего варианта (завершать программу было бы не логично).
                catch (ArgumentNullException) { }
                catch (FormatException) { }
                catch (OverflowException) { }
            }

            // При полном или частичном отсутствии корректных данных.
            if (res_line1 == "" && res_line2 == "")
            {
                throw new ArgumentException("Корректных данных в файле нет.");
            }
            else if (res_line1 == "" || res_line2 == "") // Значит что в одном из массивов 0, а в другом нет. Значит в результирующий файл надо записать 0.
            {
                WriteData(text: "0\n", path: path_output);
                throw new ArgumentException("В одном из массивов (строк входных данных) отсутствуют корректные данные. В результирующий файл записан 0.");
            }

            // Теперь это массивы очищенных данных (Не создаю новые для сокращения используемой памяти).
            line_arr_1 = res_line1[..^1].Split();
            line_arr_2 = res_line2[..^1].Split();

            // Итоговые массивы целых чисел
            int[] x_arr = new int[line_arr_1.Length];
            int[] y_arr = new int[line_arr_2.Length];

            // Запись данных в 1 массив.
            for (int i = 0; i < x_arr.Length; i++)
            {
                x_arr[i] = int.Parse(line_arr_1[i]);
            }

            // Запись данных во 2 массив.
            for (int i = 0; i < y_arr.Length; i++)
            {
                y_arr[i] = int.Parse(line_arr_2[i]);
            }

            return (x_arr, y_arr); // Возврат 2 очищенных массивов.
        }

        /// <summary>
        /// Метод назначающий и сохраняющйи номер нового файла.
        /// </summary>
        /// <returns>Возращает новый/текущий номер файла вывода(в зависимости от аргумента).</returns>
        public static int GetOutputFileNumber()
        {
            string config_path = "../../../../config.txt"; // Путь для хранения config файла.
            if (!File.Exists(config_path)) // Если его не существует.
            {
                WriteData(text: "0", path: config_path, re_write: true);
                return 0;
            }
            int number_of_file = int.Parse(File.ReadAllText(config_path, Encoding.UTF8)) + 1;
            WriteData(text: number_of_file.ToString(), path: config_path, re_write: true);
            return number_of_file; // Возвращаем номер нового файла output.txt.
        }
        /// <summary>
        /// Метод для записи результата в файл.
        /// </summary>
        /// <param name="text">Текст который нужно записать в файл.</param>
        /// <param name="path">Путь до файла.</param>
        /// <param name="re_write">Перезаписать или нет (по умолчание нет -> данные просто добавятся).</param>
        public static void WriteData(string text, string path, bool re_write=false)
        {
            try
            {
                if (re_write) // Перезаписываем.
                {
                    File.WriteAllText(path, text, Encoding.UTF8);
                }
                else // Добавляем.
                {
                    File.AppendAllText(path, text, Encoding.UTF8);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Проблемы с записью данных в файл»");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("нет прав (на изменение в пути к) на файл: " + ex.ToString());
            }
        }

    }

    /// <summary>
    /// Класс, в котором описано решение задачи и подсчета итоговой величины.
    /// </summary>
    internal static class Interactions
    {
        /// <summary>
        /// Метод подсчета S (итоговой величины) по заданной формуле.
        /// </summary>
        /// <param name="x_arr">Первый массив.</param>
        /// <param name="y_arr">Второй массив.</param>
        /// <returns>Искомая величина (S).</returns>
        private static decimal SCalculating(int[] x_arr, int[] y_arr)
        {
            decimal s = 0; // Искомая величина.
            const decimal two = 2;
            for (int i = 0; i < x_arr.Length; i++)
            {
                s += two / (x_arr[i] + y_arr[i]);
                // При делении на ноль обрабатывается исключение(В Functional_action) и просит изменить input.txt (для конкретного теста).
            }
            return s;
        }

        /// <summary>
        /// Метод выполняюший 1 итерацую решения дополнительной задачи(описана в начале и в варианте).
        /// </summary>
        public static void Functional_action()
        {
            try
            {
                // Считывание и подготовка к подсчету данных.
                string[] readText = Text_Interactions.ReadFile();

                // Провереят достаточность данных для начала обработки.
                if (readText.Length - 2 < 0)
                {
                    Console.WriteLine("Корректных данных для обработки в файле нет.");
                    return;
                }

                // Проходим по тестам и запускаем каждый из них.
                for (int i = 0; i <= readText.Length - 2; i+=2)
                {
                    try
                    {
                        // Два обработанных массива для посчета результата.
                        (int[] x_arr, int[] y_arr) = Text_Interactions.Parser(readText[i..(i+2)]);

                        // Если один из массивов больше другого, то записываем 0 в результирующий файл.
                        if (x_arr.Length != y_arr.Length)
                        {
                            Text_Interactions.WriteData(text: "0\n", path: Text_Interactions.path_output);
                            Console.WriteLine($"Тест: {i / 2}");
                            Console.WriteLine($"Массивы не равны. Результат: {0} записан в файл.");
                        }
                        else
                        {
                            // Иначе считаем по формуле и записываем результат.
                            decimal res_s = SCalculating(x_arr, y_arr);
                            Text_Interactions.WriteData(text: $"{res_s:F3}\n", path: Text_Interactions.path_output);
                            Console.WriteLine($"Тест: {i / 2} выполнен. Результат: {res_s:F3} записан в файл.");
                        }
                    }
                    catch (DivideByZeroException)
                    {
                        string error_text = "Деление на ноль при подсчете формулы (отредактируйте входные данные) -> в одном массиве +alpha/ а в другом -alpha на одинаковых местах.";
                        Text_Interactions.WriteData(text: error_text + "\n", path: Text_Interactions.path_output);
                        Console.WriteLine($"Тест: {i / 2}");
                        Console.WriteLine(error_text);
                    }
                    catch (ArgumentException ex)
                    {
                        Text_Interactions.WriteData(text: ex.Message + "\n", path: Text_Interactions.path_output);
                        Console.WriteLine($"Тест: {i / 2}");
                        Console.WriteLine(ex.Message);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"Выполнено! Все данные обработаны.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Входной Файл на диске отсутствует.");
            }
            catch (IOException)
            {
                Console.WriteLine("Проблемы с открытием файла.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("нет прав на (путь до) файл ввода. Пояснение системы:\n" + ex.ToString());
            }
        }
    }

    /// <summary>
    /// Класс, в котором описано взаимодействие пользователя с консолью.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Метод реализующий повтор ключевого алгоритма.
        /// И взаимодействие с пользователем.
        /// </summary>
        public static void Main()
        {
            ConsoleKeyInfo keyToExit; // Переменная для сохранения ключа выхода.
            Text_Interactions.path_output += $"output-{Text_Interactions.GetOutputFileNumber()}.txt"; // Генерация пути для сохранения данных.
            
            do // Цикл повторения решения.
            {
                // Удаление файла с решением, для перерасчета результата (при существовании).
                if (File.Exists(Text_Interactions.path_output))
                {
                    File.Delete(Text_Interactions.path_output);
                }
                Console.Clear();
                Console.WriteLine("Запуск -> ");
                Interactions.Functional_action();

                // Запрос на повтор решения или выход.
                Console.WriteLine("Для выхода нажмите Escape. Для повторного выполнения нажмите другю клавишу (например Enter)");
                keyToExit = Console.ReadKey();

            } while (keyToExit.Key != ConsoleKey.Escape); // Окончание цикла решения.
        }
    }
}