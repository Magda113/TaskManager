using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.BusinessLogic
{
    public class TaskManagerService
    {
        private List<Task> _tasks = new List<Task>();

        //Dodaje nowe zadanie do listy zadań z podanym opisem i opcjonalną datą realizacji.Zwraca utworzone zadanie.
        public Task Add(string description, DateTime? dueDate)
        {
            var task = new Task(description, dueDate);
            _tasks.Add(task);
            return task;
        }

        //Usuwa zadanie o podanym ID z listy zadań. Zwraca wartość bool z informacją czy udało się usunąć zadanie.
        public bool Remove(int taskId)
        {
            var task = Get(taskId);
            if (task != null)
                return _tasks.Remove(task);
            return false;
        }

        //Pobiera zadanie o podanym ID z listy zadań.Może zwrócić brak zadania.
        public Task? Get(int taskId)
        {
            return _tasks.Find(n => n.Id == taskId);
        }

        //Pobiera wszystkie zadania z listy w formie tablicy. Może zwrócić pustą tablicę.
        public Task[] GetAll()
        {
            return _tasks.ToArray();
        }

        //Pobiera wszystkie zadania w formie tablicy o podanym statusie z listy.Może zwrócić pustą tablicę.
        public Task[] GetAll(TaskStatus status)
        {
            return _tasks.FindAll(t => t.Status == status).ToArray();
        }

        //Pobiera wszystkie zadania w formie tablicy o podanym opisie z listy.Może zwrócić pustą tablicę.
        public Task[] GetAll(string description)
        {
            return _tasks.FindAll(n => n.Description.Contains(description)).ToArray();
        }
        //Zmienia status zadania o podanym ID na podany status.Zwraca wartość bool z informacją czy udało się zmienić status.
        public bool ChangeStatus(int taskId, TaskStatus newStatus)
        {
            var task = Get(taskId);
            if (task == null)
            {
                return false;
            }
            if (task?.Status == newStatus)
            {
                return false;
            }
            switch (newStatus)
            {
                case TaskStatus.ToDo:
                    return task.Open();
                case TaskStatus.InProgress:
                    return task.Start();
                case TaskStatus.Done:
                    return task.Done();
                default:
                    return false;
            }
        }
    }
}
