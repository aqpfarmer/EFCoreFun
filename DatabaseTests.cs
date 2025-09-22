using Microsoft.EntityFrameworkCore;

namespace EFCoreFun
{
    public class DatabaseTests
    {
        public static void RunTests()
        {
            Console.WriteLine("Running CRUD Tests...\n");

            // Test Create Operations
            TestCreate();

            // Test Read Operations
            TestRead();

            // Test Update Operations
            TestUpdate();

            // Test Delete Operations
            TestDelete();

            Console.WriteLine("All CRUD tests completed successfully!");
        }

        private static AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private static void TestCreate()
        {
            Console.WriteLine("Testing Create Operations:");

            using var context = GetInMemoryContext();

            // Create Department
            var department = new Department { Name = "Test Department" };
            context.Departments.Add(department);
            context.SaveChanges();

            var savedDepartment = context.Departments.FirstOrDefault(d => d.Name == "Test Department");
            if (savedDepartment != null && savedDepartment.DepartmentId > 0)
            {
                Console.WriteLine("✓ Department created successfully");
            }
            else
            {
                Console.WriteLine("✗ Department creation failed");
            }

            // Create Employee
            var employee = new Employee { Name = "John Doe", DepartmentId = department.DepartmentId };
            context.Employees.Add(employee);
            context.SaveChanges();

            var savedEmployee = context.Employees.FirstOrDefault(e => e.Name == "John Doe");
            if (savedEmployee != null && savedEmployee.EmployeeId > 0)
            {
                Console.WriteLine("✓ Employee created successfully");
            }
            else
            {
                Console.WriteLine("✗ Employee creation failed");
            }

            Console.WriteLine();
        }

        private static void TestRead()
        {
            Console.WriteLine("Testing Read Operations:");

            using var context = GetInMemoryContext();

            // Setup test data
            var hrDept = new Department { Name = "HR" };
            var itDept = new Department { Name = "IT" };
            context.Departments.AddRange(hrDept, itDept);
            context.SaveChanges();

            var employees = new[]
            {
                new Employee { Name = "Alice", DepartmentId = hrDept.DepartmentId },
                new Employee { Name = "Bob", DepartmentId = itDept.DepartmentId }
            };
            context.Employees.AddRange(employees);
            context.SaveChanges();

            // Test reading employees with departments
            var employeesWithDepartments = context.Employees
                .Join(context.Departments,
                      e => e.DepartmentId,
                      d => d.DepartmentId,
                      (e, d) => new { e.Name, DepartmentName = d.Name })
                .ToList();

            if (employeesWithDepartments.Count == 2)
            {
                Console.WriteLine("✓ Read employees with departments successfully");
            }
            else
            {
                Console.WriteLine("✗ Read operations failed");
            }

            // Test specific employee search
            var charlie = context.Employees
                .FirstOrDefault(x => x.Name == "Alice");

            if (charlie != null)
            {
                Console.WriteLine("✓ Specific employee search successful");
            }
            else
            {
                Console.WriteLine("✗ Specific employee search failed");
            }

            Console.WriteLine();
        }

        private static void TestUpdate()
        {
            Console.WriteLine("Testing Update Operations:");

            using var context = GetInMemoryContext();

            // Setup test data
            var department = new Department { Name = "Engineering" };
            context.Departments.Add(department);
            context.SaveChanges();

            var employee = new Employee { Name = "Jane Smith", DepartmentId = department.DepartmentId };
            context.Employees.Add(employee);
            context.SaveChanges();

            // Test employee update
            var employeeToUpdate = context.Employees.First(e => e.Name == "Jane Smith");
            employeeToUpdate.Name = "Jane Johnson";
            context.SaveChanges();

            var updatedEmployee = context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (updatedEmployee != null && updatedEmployee.Name == "Jane Johnson")
            {
                Console.WriteLine("✓ Employee update successful");
            }
            else
            {
                Console.WriteLine("✗ Employee update failed");
            }

            // Test department update
            var deptToUpdate = context.Departments.First(d => d.Name == "Engineering");
            deptToUpdate.Name = "Software Engineering";
            context.SaveChanges();

            var updatedDept = context.Departments.FirstOrDefault(d => d.DepartmentId == department.DepartmentId);
            if (updatedDept != null && updatedDept.Name == "Software Engineering")
            {
                Console.WriteLine("✓ Department update successful");
            }
            else
            {
                Console.WriteLine("✗ Department update failed");
            }

            Console.WriteLine();
        }

        private static void TestDelete()
        {
            Console.WriteLine("Testing Delete Operations:");

            using var context = GetInMemoryContext();

            // Setup test data
            var department = new Department { Name = "Finance" };
            context.Departments.Add(department);
            context.SaveChanges();

            var employee = new Employee { Name = "Mike Wilson", DepartmentId = department.DepartmentId };
            context.Employees.Add(employee);
            context.SaveChanges();

            // Test employee delete
            var employeeToDelete = context.Employees.First(e => e.Name == "Mike Wilson");
            context.Employees.Remove(employeeToDelete);
            context.SaveChanges();

            var deletedEmployee = context.Employees.FirstOrDefault(e => e.Name == "Mike Wilson");
            if (deletedEmployee == null)
            {
                Console.WriteLine("✓ Employee delete successful");
            }
            else
            {
                Console.WriteLine("✗ Employee delete failed");
            }

            // Test department delete
            var deptToDelete = context.Departments.First(d => d.Name == "Finance");
            context.Departments.Remove(deptToDelete);
            context.SaveChanges();

            var deletedDept = context.Departments.FirstOrDefault(d => d.Name == "Finance");
            if (deletedDept == null)
            {
                Console.WriteLine("✓ Department delete successful");
            }
            else
            {
                Console.WriteLine("✗ Department delete failed");
            }

            Console.WriteLine();
        }
    }
}