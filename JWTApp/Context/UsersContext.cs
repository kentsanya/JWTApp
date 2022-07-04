using JWTApp.Models;

namespace JWTApp.Context
{
    public class UsersContext
    {
        public List<Person> Users { get; set; } = new List<Person>()
        {
            new Person () {Name="Kolya", Password="12345"},
            new  Person () {Name="Anton" , Password="Qwerty"}
        };
    }
}
