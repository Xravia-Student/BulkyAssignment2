using Bulky.Models;
using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}

//Defining the IRepository<T> Interface​

//The IRepository<T> interface will include method signatures for essential CRUD operations. ​
//Here's what each method will do:​
//GetAll() – Retrieves all categories.​
//Get(id) – Fetches a specific category by its ID.​
//Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id); ​
//Add(T entity) – Creates a new category.​
//Remove(T entity) – Deletes a single category.​
//RemoveRange(IEnumerable<T> entities) – Deletes multiple categories at once.
//By defining these methods in the IRepository<T> interface, we establish a contract that any class implementing this interface must follow. This promotes consistency and allows for easier maintenance and scalability of our data access layer.
// T Get(Expression<Func<T, bool>> filter);​
//Func<T, bool> is a function(lambda expression) that:​
//   Takes an object of type T and returns true or false​
//Using Expression means that it is an expression tree that will allow Entity Framework(or LINQ providers) to​
//    Convert the condition into SQL and then Execute it in the database​
//filter -> Represents the condition used to find the record​
//Translation is ​Select * from Categories where Name='Action'