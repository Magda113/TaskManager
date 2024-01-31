using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.BusinessLogic
{
    public class TaskManagerService
    {
        private List<TaskItem> _tasks = new List<TaskItem>();

        private static int _id =0;

        //Dodaje nowe zadanie do listy zadań z podanym opisem i opcjonalną datą realizacji.Zwraca utworzone zadanie.
        public async Task<TaskItem> AddAsync(string description, int createdBy, DateTime? dueDate)
        {
            _id++;
            var task = new TaskItem(0, description,new User(1, "test"), dueDate);
            _tasks.Add(task);
            return task;
        }
        
        //Usuwa zadanie o podanym ID z listy zadań. Zwraca wartość bool z informacją czy udało się usunąć zadanie.
        public async Task<bool> RemoveAsync(int taskId)
        {
            var task = GetAsync(taskId);
            if (task != null)
                return _tasks.Remove(await task);
            return false;
        }

        //Pobiera zadanie o podanym ID z listy zadań.Może zwrócić brak zadania.
        public async Task<TaskItem?> GetAsync(int taskId)
        {
            return _tasks.Find(n => n.Id == taskId);
        }

        //Pobiera wszystkie zadania z listy w formie tablicy. Może zwrócić pustą tablicę.
        public async Task<TaskItem[]> GetAllAsync()
        {
            return _tasks.ToArray();
        }

        //Pobiera wszystkie zadania w formie tablicy o podanym statusie z listy.Może zwrócić pustą tablicę.
        public async Task<TaskItem[]> GetAllAsync(TaskItemStatus status)
        {
            return _tasks.FindAll(t => t.Status == status).ToArray();
        }

        //Pobiera wszystkie zadania w formie tablicy o podanym opisie z listy.Może zwrócić pustą tablicę.
        public async Task<TaskItem[]> GetAllAsync(string description)
        {
            return _tasks.FindAll(n => n.Description.Contains(description)).ToArray();
        }
        //Zmienia status zadania o podanym ID na podany status.Zwraca wartość bool z informacją czy udało się zmienić status.
        public async Task<bool> ChangeStatusAsync(int taskId, TaskItemStatus newStatus)
        {
            var task = await GetAsync(taskId);
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
                case BusinessLogic.TaskItemStatus.ToDo:
                    return task.Open();
                case BusinessLogic.TaskItemStatus.InProgress:
                    return task.Start();
                case BusinessLogic.TaskItemStatus.Done:
                    return task.Done();
                default:
                    return false;
            }
        }
        
    }
}
