
using EFCoreFun;
using System.Linq;


using (var context = new AppDbContext())
{
	// Ensure database is created
	context.Database.EnsureCreated();

	// Add Departments if not exist
	if (!context.Departments.Any())
	{
		var hr = new Department { Name = "HR" };
		var engineering = new Department { Name = "Engineering" };
		var development = new Department { Name = "Development" };
		context.Departments.AddRange(hr, engineering, development);
		context.SaveChanges();

		// Add Employees
		var employees = new[]
		{
			new Employee { Name = "Alice", DepartmentId = hr.DepartmentId },
			new Employee { Name = "Bob", DepartmentId = engineering.DepartmentId },
			new Employee { Name = "Charlie", DepartmentId = development.DepartmentId },
			new Employee { Name = "Diana", DepartmentId = engineering.DepartmentId },
			new Employee { Name = "Eve", DepartmentId = hr.DepartmentId }
		};
		context.Employees.AddRange(employees);
		context.SaveChanges();
		Console.WriteLine("Sample data added.");
	}

	// Query and display all employees with their department names
	var employeesWithDepartments = context.Employees
		.Join(context.Departments,
			  e => e.DepartmentId,
			  d => d.DepartmentId,
			  (e, d) => new { e.Name, DepartmentName = d.Name })
		.ToList();

	Console.WriteLine("Employees and their Departments:");
	foreach (var item in employeesWithDepartments)
	{
		Console.WriteLine($"{item.Name} - {item.DepartmentName}");
	}

	// Query and display the employee named 'Charlie'
	var charlie = context.Employees
		.Join(context.Departments,
			  e => e.DepartmentId,
			  d => d.DepartmentId,
			  (e, d) => new { e.Name, DepartmentName = d.Name })
		.FirstOrDefault(x => x.Name == "Charlie");

	if (charlie != null)
	{
		Console.WriteLine($"\nRecord for 'Charlie':");
		Console.WriteLine($"{charlie.Name} - {charlie.DepartmentName}. Our mission was a success!");
	}
	else
	{
		Console.WriteLine("\nNo record found for 'Charlie'.");
	}
}

Console.WriteLine("\n" + new string('=', 50));
Console.WriteLine("Running CRUD Tests:");
Console.WriteLine(new string('=', 50));

// Run CRUD tests
DatabaseTests.RunTests();
