using System.Data.Entity;
using CSESoftware.Repository.EntityFramework;

namespace CSESoftware.RepositoryTestProject.Setup
{
    public class DbContext : BaseMockDbContext
    {
        public DbContext(string databaseId) : base(databaseId)
        {
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Crust> Crusts { get; set; }
    }
}