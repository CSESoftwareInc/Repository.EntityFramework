# CSESoftware.Repository.EntityFramework

The Entity Framework implementation of CSESoftware.Repository.

---

To use this:
* create your own DbContext
* inherit from the BaseDbContext
* add your DbSets
* setup Dependency Injection


```C#
public class DbContext : BaseMockDbContext
{
 	public DbContext(string connectionString) : base(connectionString)
	{
	}

	public DbSet<Pizza> Pizzas { get; set; }
	public DbSet<Topping> Toppings { get; set; }
	public DbSet<Crust> Crusts { get; set; }
}
```


All of your entites should inherit from CSESoftware.Core.Entity<>
```C#
public class Pizza : BaseEntity<int>
{
	public string Name { get; set; }
	public double Cost { get; set; }

	public int CrustId { get; set; }
	public Crust Crust { get; set; }

	public int ToppingId { get; set; }
	public Topping Topping { get; set; }
}
```


Unity Dependency Injection Example
```C#
container.RegisterType<DbContext, ExampleDbContext>();
container.RegisterType<IRepository, CSESoftware.Repository.EntityFramework.Repository<ExampleDbContext>>();
container.RegisterType<IReadOnlyRepository, CSESoftware.Repository.EntityFramework.ReadOnlyRepository<ExampleDbContext>>();
```

---

CSE Software Inc. is a privately held company founded in 1990. CSE develops software, AR/VR, simulation, mobile, and web technology solutions. The company also offers live, 24x7, global help desk services in 110 languages. All CSE teams are U.S. based with experience in multiple industries, including government, military, healthcare, construction, agriculture, mining, and more. CSE Software is a certified women-owned small business. Visit us online at [csesoftware.com](csesoftware.com).
