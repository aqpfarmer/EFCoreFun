using System.Collections.Generic;

namespace EFCoreFun
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
