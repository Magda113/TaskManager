using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic;
using Task = TaskManager.BusinessLogic.Task;

namespace TaskManager.Tests
{
    public class TaskTests
    {
        [Fact]
        //Automatyczna inkrementacja ID: Testuje, czy każde nowo utworzone zadanie otrzymuje unikalny, autoinkrementowany identyfikator.
        public void Should_CreateTask_WithAutoIncrementedId()
        {
            // Arrange
            var task1 = new Task("test", null);
            var task2 = new Task("test", null);

            // Act
           

            // Assert
            Assert.Equal(task1.Id + 1, task2.Id);
        }

        [Fact]
        //Ustawianie daty utworzenia: Weryfikuje, czy przy tworzeniu zadania poprawnie ustawiana jest data jego utworzenia.
        public void Should_SetCreationDate_WhenCreatingTask()
        {
            //Arrange
            var task2 = new Task("test", null);

            //Act
            var difference = DateTime.Now - task2.CreationDate;

            //Assert
            Assert.True(difference.TotalSeconds < 1);
        }

        [Fact]
        //Ustawianie daty zakończenia: Sprawdza, czy możliwe jest ustawienie daty zakończenia zadania podczas jego tworzenia.
        public void Should_SetDueDate_WhenProvided()
        {
            //Arrange
            var task3 = new Task("test", DateTime.Now.AddDays(5));
            //Act

            //Assert
            Assert.True(task3.DueDate.HasValue);
        }
        [Fact]
        //Ustawienie statusu na_ToDo: Upewnia się, że nowo utworzone zadanie domyślnie ma status_ToDo

        public void Should_SetStatusToTodo_WhenTaskIsCreated()
        {
            //Arrange
            var task1 = new Task("test", null);

            //Act

            //Assert
            Assert.Equal("ToDo", task1.Status.ToString());

        }

        [Fact]
        //Zmiana statusu na InProgress: Testuje funkcję rozpoczęcia zadania i weryfikuje, czy status zadania zmienia się odpowiednio.
        public void Should_ChangeStatus_ToInProgress_WhenStartIsCalled()
        {
            //Arrange
            var task = new Task("test", null);
            task.Start();
            //Act

            //Assert
            Assert.Equal("InProgress", task.Status.ToString());
        }


    }
    
}
