using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.BusinessLogic
{
    public class TaskItem
    {
        private User _createdBy;
        private User? _assignedTo;
        public User CreatedBy => _createdBy;
        public User? AssignedTo => _assignedTo;

        private static int _id = 1; //Automatycznie zwiększa się przy tworzeniu nowego zadania.
        public int Id { get; }
        public string Description { get; set; } //Opis zadania(wartość wymagana).

        public DateTime
            CreationDate
        {
            get;
        } //Data i czas utworzenia zadania. Wartość ustawia się automatycznie przy tworzeniu zadania.

        public DateTime? DueDate { get; set; } //Opcjonalna data i czas, do kiedy zadanie powinno być zakończone.
        public DateTime? StartDate { get; private set; } //Opcjonalna data i czas rozpoczęcia zadania.
        public DateTime? DoneDate { get; private set; } //Opcjonalna data i czas zakończenia zadania.

        public TimeSpan? Duration =>
            StartDate != null
                ? (DoneDate ?? DateTime.Now) - StartDate.Value
                : null; //Czas trwania zadania (różnica między StartDate a obecnym czasem lub DoneDate jeśli zadanie zostało zakończone). Jest to wartość obliczana na żądanie.

        public TaskItemStatus Status { get; private set; } =
            TaskItemStatus.ToDo; //Aktualny status zadania. Wartość wymagana i ustawiana automatycznie przy tworzeniu zadania na wartośćToDo.

        public TaskItem(string description, DateTime? dueDate)
        {
           
            CreationDate = DateTime.Now;
            Description = description;
            DueDate = dueDate;
        }

        //Start() : Rozpoczyna zadanie, zmienia jego status na InProgress i ustawia StartDate oraz usuwa datę DoneDate. Można rozpocząć zadanie tylko wtedy, gdy już nie zostało wcześniej rozpoczęte. Zwraca wartość bool z informacją czy udało się zmienić status.
        public bool Start()
        {
            if (Status == TaskItemStatus.ToDo)
            {
                Status = TaskItemStatus.InProgress;
                StartDate = DateTime.Now;
                DoneDate = null;
                return true;
            }
            return false;
        }

        //Open() : Cofa zadanie do wykonania, zmienia jego status na ToDoi usuwa daty StartDate i DoneDate. Można cofnąć zadanie tylko wtedy, gdy nie jest ono w statusie_ToDo. Zwraca wartość bool z informacją czy udało się zmienić status.

        public bool Open()
        {
            if (Status != TaskItemStatus.ToDo)
            {
                Status = TaskItemStatus.ToDo;
                StartDate = null;
                DoneDate = null;
                return true;
            }
            return false;
        }

        //Done() : Kończy zadanie, zmienia jego status na Done i ustawia DoneDate.Zadanie można zakończyć tylko wtedy gdy zostało rozpoczęte.Zwraca wartość bool z informacją czy udało się zmienić status.

        public bool Done()
        {
            if (Status == TaskItemStatus.InProgress)
            {
                Status = TaskItemStatus.Done;
                DoneDate = DateTime.Now;
                return true;
            }
            return false;
        }
        //ToString(): Wyświetla przyjazny i krótki opis zadania w formacie ID - Opis (Status).

        public override string ToString()
        {
            return $"{Id} - {Description} ({Status}) @{AssignedTo?.Name ?? "nieprzypisane"}";
        }
        public void AssignTo(User? assignedTo)
        {
            _assignedTo = assignedTo;
        }
        private TaskItem()
        {
        }

        public TaskItem(int id, string description, User createdBy, DateTime? dueDate)
        {
            Id = id;
            Description = description;
            CreationDate = DateTime.Now;
            _createdBy = createdBy;
            DueDate = dueDate;
        }

    }

}
