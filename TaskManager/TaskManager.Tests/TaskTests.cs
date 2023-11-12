using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic;
using Task = TaskManager.BusinessLogic.Task;
using TaskStatus = TaskManager.BusinessLogic.TaskStatus;

namespace TaskManager.Tests
{
    public class TaskTests
    {
        [Fact]
        //1.Automatyczna inkrementacja ID: Testuje, czy każde nowo utworzone zadanie otrzymuje unikalny, autoinkrementowany identyfikator.
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
        //2.Ustawianie daty utworzenia: Weryfikuje, czy przy tworzeniu zadania poprawnie ustawiana jest data jego utworzenia.
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
        //3.Ustawianie daty zakończenia: Sprawdza, czy możliwe jest ustawienie daty zakończenia zadania podczas jego tworzenia.
        public void Should_SetDueDate_WhenProvided()
        {
            //Arrange
            var task3 = new Task("test", DateTime.Now.AddDays(5));
            //Act

            //Assert
            Assert.True(task3.DueDate.HasValue);
        }
        [Fact]
        //4.Ustawienie statusu na_ToDo: Upewnia się, że nowo utworzone zadanie domyślnie ma status_ToDo

        public void Should_SetStatusToTodo_WhenTaskIsCreated()
        {
            //Arrange
            var task1 = new Task("test", null);

            //Act

            //Assert
            Assert.Equal("ToDo", task1.Status.ToString());

        }

        [Fact]
        //5.Zmiana statusu na InProgress: Testuje funkcję rozpoczęcia zadania i weryfikuje, czy status zadania zmienia się odpowiednio.
        public void Should_ChangeStatus_ToInProgress_WhenStartIsCalled()
        {
            //Arrange
            var task = new Task("test", null);
           
            //Act
            task.Start();
            //Assert
            Assert.Equal("InProgress", task.Status.ToString());
        }

        [Fact]
        //6.Ustawienie daty rozpoczęcia: Sprawdza, czy po rozpoczęciu zadania ustawiana jest odpowiednia data rozpoczęcia.
        public void Should_SetStartDate_WhenStartIsCalled()
        {
            //Arrange
            var task = new Task("test", null);

            //Act
            task.Start();
            TimeSpan difference = DateTime.Now - task.StartDate.Value;
            //Assert
            Assert.True(difference.TotalSeconds < 1);
        }

        [Fact]
        //7.Niezmienność statusu przy ponownym rozpoczęciu: Upewnia się, że zadanie, które jest już w trakcie realizacji, nie może być ponownie rozpoczęte.
        public void Should_NotChangeStatus_ToInProgress_IfAlreadyInProgress()
        {
            //Arrange
            var task = new Task("test", null);
           
            //Act
            task.Start();
            bool result = task.Start();

            //Assert
            Assert.False(result);

        }

        [Fact]
        //8.Zmiana statusu na Done: Weryfikuje, czy zadanie w trakcie realizacji można oznaczyć jako zakończone i czy status zadania zmienia się odpowiednio.
        public void Should_ChangeStatus_ToDone_WhenDoneIsCalledAndStatusIsInProgress()
        {
            //Arrange
            var task = new Task("test", null);

            //Act
            task.Start();
            bool result = task.Done();

            //Asert
            Assert.True(result);
            Assert.Equal("Done", task.Status.ToString());
        }

        [Fact]
        //9.Ustawienie daty zakończenia: Sprawdza, czy po zakończeniu zadania ustawiana jest odpowiednia data zakończenia.
        public void Should_SetDoneDate_WhenDoneIsCalled()
        {
            //Arrange
            var task = new Task("test", null);

            //Act
            task.Start();
            task.Done();
            TimeSpan difference = DateTime.Now - task.DoneDate.Value;

            //Assert
            Assert.True(difference.TotalSeconds < 1);
        }

        [Fact]
        //10.Niezmienność statusu bez rozpoczęcia: Upewnia się, że zadanie, które nie zostało rozpoczęte, nie można oznaczyć jako zakończone.
        public void Should_NotChangeStatus_ToDone_IfStatusIsNotInProgress()
        {
            //Arrange
            var task = new Task("test", null);

            //Act
            bool result = task.Done();

            //Assert
            Assert.False(result);
            Assert.Equal("ToDo", task.Status.ToString());
        }

        [Fact]
        //11.Obliczanie czasu trwania: Testuje obliczanie czasu trwania zadania, które jest w trakcie realizacji.
        public void Should_CalculateDuration_WhenStatusIsInProgress()
        {
            //Arrange
            var task = new Task("test", null);

            //Act
            task.Start();


            //Assert
            Assert.True(task.Duration.Value.TotalSeconds> 0);
            Assert.NotNull(task.Duration);
        }

        [Fact]
        //12.Brak czasu trwania dla zadań_ToDo: Weryfikuje, że zadanie z statusem_ToDo nie ma określonego czasu trwania.
        public void Should_ReturnNullDuration_WhenStatusIsTodo()
        {
            //Arrange
            var task = new Task("test", null);

            //Act
            
            //Assert
            Assert.Null(task.Duration);
        }

    }

}
