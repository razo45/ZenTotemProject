using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZenTotem;
using System;
using System.Collections.Generic;

namespace ZenTotemProject.UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        
        [TestMethod]
        public void AddEmployee_ValidArguments_EmployeeAddedToList()
        {
            // Arrange
            Program.employees = new List<Employee>(); // Создаем новый список сотрудников

            var args = new string[] { "-add", "firstName:John", "lastName:Doe", "salary:1000" };

            // Act
            Program.AddEmployee(args);

            // Assert
            Assert.AreEqual(1, Program.employees.Count);
            Assert.AreEqual("John", Program.employees[0].FirstName);
            Assert.AreEqual("Doe", Program.employees[0].LastName);
            Assert.AreEqual(1000, Program.employees[0].SalaryPerHour);


        }
    }
}
