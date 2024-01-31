using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.BusinessLogic
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TaskItem> Tasks { get; }
        
        private User()
        {
        }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
            Tasks = new List<TaskItem>();
        }

        public override string ToString() => $"{Id}. {Name}";
    }
}
