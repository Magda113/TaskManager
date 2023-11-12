using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic;
using TaskStatus = TaskManager.BusinessLogic.TaskStatus;

namespace TaskManager
{
    public class Program
    {
        private static TaskManagerService _taskManagerService = new TaskManagerService();
        static void Main(string[] args)
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
                            var task = _taskManagerService.Add(description, dueDate);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Dodano zadanie");
                            Console.ResetColor();

                        }

                        if (answer1 == "nie")
                        {
                            var task = _taskManagerService.Add(description, null);
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

                        if (_taskManagerService.Remove(id))
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

                        var task1 = _taskManagerService.Get(idToShow);
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
                        var tasks = _taskManagerService.GetAll();
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
                                var tasksToDo = _taskManagerService.GetAll(TaskStatus.ToDo);
                                Console.WriteLine($"Masz {tasksToDo.Length} zadań do zrobienia:");
                                foreach (var task in tasksToDo)
                                {
                                    Console.WriteLine(task);
                                }

                                break;
                            case "InProgress":
                                var tasksInProgress = _taskManagerService.GetAll(TaskStatus.InProgress);
                                Console.WriteLine($"Masz {tasksInProgress.Length} zadań w trakcie robienia:");
                                foreach (var task in tasksInProgress)
                                {
                                    Console.WriteLine(task);
                                }

                                break;
                            case "Done":
                                var tasksDone = _taskManagerService.GetAll(TaskStatus.Done);
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

                        var tasksToFind = _taskManagerService.GetAll(answer3);
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
                        var status = Enum.Parse(typeof(TaskStatus), Console.ReadLine(), true);
                        if (_taskManagerService.ChangeStatus(idToChange, (TaskStatus)status))
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