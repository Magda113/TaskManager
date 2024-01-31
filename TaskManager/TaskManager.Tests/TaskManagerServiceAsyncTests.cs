using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using TaskManager.BusinessLogic;
using Task = System.Threading.Tasks.Task;
using System.Collections;

namespace TaskManager.Tests
{
    public class TaskManagerServiceAsyncTests
    {
        private readonly int _createdBy = 1;

        [Fact]
        //1. Dodawanie zadania: Weryfikuje, czy nowe zadanie może być dodane do listy zadań i czy jest poprawnie zwracane po dodaniu.
        public async Task Should_AddTask_ToTaskList()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();

            // Act
            var task = await taskManagerService.AddAsync("test", _createdBy, null);

            // Assert
            Assert.True(task != null);
        }

        [Fact]
        //2. Usuwanie zadania: Testuje, czy zadanie można usunąć z listy za pomocą jego identyfikatora.
        public async Task Should_RemoveTask_ByTaskId()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();
            var task =await taskManagerService.AddAsync("test", _createdBy, null);
            // Act
            var result = await taskManagerService.RemoveAsync(task.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        //3. Niepowodzenie usuwania nieistniejącego zadania: Sprawdza, czy próba usunięcia zadania o nieistniejącym ID kończy się niepowodzeniem.
        public async Task Should_NotRemoveTask_WhenTaskIdDoesNotExist()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();
            var task =await taskManagerService.AddAsync("test", _createdBy, null);
            // Act
            var result = await taskManagerService.RemoveAsync(10);

            // Assert
            Assert.False(result);
        }

        [Fact]
        //4. Pobieranie zadania po ID: Weryfikuje, czy możliwe jest pobranie zadania z listy na podstawie jego ID.
        public async Task Should_GetTask_ByTaskId()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();
            var task = await taskManagerService.AddAsync("test", _createdBy, null);
            // Act
            var result =await taskManagerService.GetAsync(task.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        // 5. Pobieranie wszystkich zadań: Testuje funkcję, która zwraca wszystkie zadania z listy.
        public async Task Should_GetAllTasks_WithNoFilter()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();
            var task = await taskManagerService.AddAsync("test", _createdBy, null);
            // Act
            var result = await taskManagerService.GetAllAsync();

            // Assert
            Assert.Equal(result.Length,1);
        }

        [Fact]
        // 6. Filtrowanie zadań według statusu: Upewnia się, że zadania mogą być filtrowane i zwracane na podstawie ich statusu.
        public async Task Can_GetTask_ByTaskId()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();
            var task = await taskManagerService.AddAsync("test", _createdBy, null);
            // Act
            var result = await taskManagerService.GetAllAsync(TaskItemStatus.ToDo);

            // Assert
            Assert.Equal(result.Length, 1);
        }
        [Fact]
        //7. Filtrowanie zadań według opisu: Weryfikuje, czy zadania można filtrować na podstawie słów kluczowych w ich opisie.
        public async Task Should_GetTasks_ByDescription()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();
            var task1 = await taskManagerService.AddAsync("test", _createdBy, null);
            var task2 = await taskManagerService.AddAsync("test", _createdBy, null);
            // Act
            var result = await taskManagerService.GetAllAsync("test");

            // Assert
            Assert.Equal(result.Length, 2);
        }

        [Fact]
        //8. Zmiana statusu zadania: Sprawdza, czy status zadania można zmienić, pod warunkiem, że jest to dozwolone.
        public async Task Should_ChangeTaskStatus_WhenValid()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();
            var task = await taskManagerService.AddAsync("test", _createdBy, null);
            
            // Act
            var result = await taskManagerService.ChangeStatusAsync(task.Id, TaskItemStatus.InProgress);

            // Assert
            Assert.True(result);
        }
        [Fact]
        //9. Nieudana zmiana statusu zadania z powodu nieprawidłowej sekwencji: Testuje, czy próba nieprawidłowej zmiany statusu (np. bezpośrednio z "ToDo" na "Done") kończy się niepowodzeniem.
        public async Task Should_NotChangeTaskStatus_WhenInvalidTransition()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();
            var task = await taskManagerService.AddAsync("test", _createdBy, null);

            // Act
            var result = await taskManagerService.ChangeStatusAsync(task.Id, TaskItemStatus.Done);

            // Assert
            Assert.False(result);
        }
        [Fact]
        // 10.Nieudana zmiana statusu dla nieistniejącego zadania: Weryfikuje, czy próba zmiany statusu dla nieistniejącego zadania kończy się niepowodzeniem.
        public async Task Should_NotChangeTaskStatus_WhenTaskIdDoesNotExist()
        {
            // Arrange
            var taskManagerService = new TaskManagerService();
            var task = await taskManagerService.AddAsync("test", _createdBy, null);

            // Act
            var result = await taskManagerService.ChangeStatusAsync(10, TaskItemStatus.InProgress);

            // Assert
            Assert.False(result);
        }

    }
}
