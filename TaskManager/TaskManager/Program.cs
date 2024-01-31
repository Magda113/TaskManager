using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic;

namespace TaskManager
{
    public class Program
    {
        private const string ConnectionString = "Persist Security Info=False;Integrated Security=true; TrustServerCertificate=True; Initial Catalog=TaskManager;Server=LAPTOP\\SQLEXPRESS";
        private static int _createdBy = 1;

        
        private static TaskManagerService _taskManagerService = new TaskManagerService();
        static async Task Main(string[] args)
        {
            
            int answer;
            do
            {

                Console.WriteLine($@"Wybierz opcję z listy:
            1. Dodaj zadanie
            2. Usuń zadanie
            3. Pokaż szczegóły zadania
            4. Wyświetl wszystkie zadania
            5. Wyświetl zadania według statusu
            6. Szukaj zadania
            7. Zmień status zadania
            8. Zakończ");
                answer = int.Parse(Console.ReadLine());
                switch (answer)
                {
                    case 1:
                        Console.WriteLine("Podaj opis zadania");
                        var description = Console.ReadLine();
                        Console.WriteLine("Chcesz podać datę zakończenia zadania? (tak/nie)");
                        var answer1 = Console.ReadLine();
                        if (answer1 != "tak" && answer1 != "nie")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("coś poszło nie tak");
                            Console.ResetColor();
                        }

                        if (answer1 == "tak")
                        {
                            Console.WriteLine("Podaj tę datę");
                            DateTime dueDate = DateTime.Parse(Console.ReadLine());
                            var task = await _taskManagerService.AddAsync(description, _createdBy, dueDate);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Dodano zadanie");
                            Console.ResetColor();

                        }

                        if (answer1 == "nie")
                        {
                            var task = await _taskManagerService.AddAsync(description, _createdBy, null);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Dodano zadanie");
                            Console.ResetColor();
                        }

                        break;
                    case 2:
                        Console.WriteLine("Podaj numer zadania do usunięcia");
                        int id;
                        while (!int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.WriteLine("Spróbuj jeszcze raz");
                        }

                        if (await _taskManagerService.RemoveAsync(id))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Usunięto zadanie");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Nie udało się usunąć zadania.");
                            Console.ResetColor();
                        }

                        break;
                    case 3:
                        Console.WriteLine("Podaj numer zadania do wyświetlenia");
                        int idToShow;
                        while (!int.TryParse(Console.ReadLine(), out idToShow))
                        {
                            Console.WriteLine("Spróbuj jeszcze raz");
                        }

                        var task1 = await _taskManagerService.GetAsync(idToShow);
                        if (task1 != null)
                        {
                            var sb = new StringBuilder();
                            sb.AppendLine(task1.ToString());
                            sb.AppendLine($"  Data utworzenia: {task1.CreationDate}");
                            sb.AppendLine($"  Data spodziewanego końca: {task1.DueDate}");
                            sb.AppendLine($"  Data startu: {task1.StartDate}");
                            sb.AppendLine($"  Data zakończenia: {task1.DoneDate}");
                            sb.AppendLine($"  Czas trwania: {task1.Duration}");
                            Console.WriteLine(sb);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Takiego zadania nie ma na liście.");
                            Console.ResetColor();
                        }

                        break;
                    case 4:
                        var tasks = await _taskManagerService.GetAllAsync();
                        Console.WriteLine($"Masz {tasks.Length} zadań:");
                        foreach (var task in tasks)
                        {
                            Console.WriteLine(task);
                        }

                        break;
                    case 5:
                        Console.WriteLine("Podaj status zadań do wyświetlenia (ToDo/ InProgress/ Done");
                        var answer2 = Console.ReadLine();
                        switch (answer2)
                        {
                            case "ToDo":
                                var statuses = string.Join(", ", Enum.GetNames<TaskItemStatus>());
                                Console.WriteLine($"Podaj status: {statuses}");
                                TaskItemStatus itemStatus;
                                while (!Enum.TryParse<TaskItemStatus>(Console.ReadLine(), true, out itemStatus))
                                {
                                    Console.WriteLine($"Podaj status: {statuses}");
                                }
                                var tasksToDo = await _taskManagerService.GetAllAsync(TaskItemStatus.ToDo);
                                Console.WriteLine($"Masz {tasksToDo.Length} zadań do zrobienia:");
                                foreach (var task in tasksToDo)
                                {
                                    Console.WriteLine(task);
                                }
                               

                                break;
                            case "InProgress":
                                var tasksInProgress =await _taskManagerService.GetAllAsync(TaskItemStatus.InProgress);
                                Console.WriteLine($"Masz {tasksInProgress.Length} zadań w trakcie robienia:");
                                foreach (var task in tasksInProgress)
                                {
                                    Console.WriteLine(task);
                                }

                                break;
                            case "Done":
                                var tasksDone = await _taskManagerService.GetAllAsync(TaskItemStatus.Done);
                                Console.WriteLine($"Masz {tasksDone.Length} zadań zakończonych:");
                                foreach (var task in tasksDone)
                                {
                                    Console.WriteLine(task);
                                }

                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("coś poszło nie tak");
                                Console.ResetColor();
                                break;
                        }

                        break;
                    case 6:
                        Console.WriteLine("Podaj opis do wyszukwania:");
                        var answer3 = Console.ReadLine();
                        while (true)
                        {
                            if (string.IsNullOrEmpty(answer3))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("coś poszło nie tak, spróbuj jeeszcze raz.");
                                Console.ResetColor();
                                continue;
                            }

                            break;
                        }

                        var tasksToFind = await _taskManagerService.GetAllAsync(answer3);
                        Console.WriteLine();
                        Console.WriteLine($"Znaleziono {tasksToFind.Length} zadań:");
                        foreach (var task in tasksToFind)
                        {
                            Console.WriteLine(task);
                        }

                        break;
                    case 7:
                        Console.WriteLine("Podaj numer zadania do zmienienia");
                        var idToChange = int.Parse(Console.ReadLine());
                        Console.WriteLine("Podaj nowy status (ToDo/InProgress/Done:");
                        var status = Enum.Parse(typeof(TaskItemStatus), Console.ReadLine(), true);
                        if (await _taskManagerService.ChangeStatusAsync(idToChange, (TaskItemStatus)status))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Udało się zmienić status");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("coś poszło nie tak.");
                            Console.ResetColor();
                        }

                        break;
                    
                }

            }
            while (answer != 8) ;





        }
    }
}