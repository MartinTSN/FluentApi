using System;
using System.Linq;
using FluentApi.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentApi.Tests
{
    [TestClass]
    public class EmployeeTests
    {
        [TestMethod]
        public void GetAllEmployees()
        {
            Model model = new Model();
            var results = model.Employees.ToList();

            Assert.AreNotEqual(results.Count, 0);
        }

        //[TestMethod]
        //public void AddNewEmployee()
        //{
        //    Model model = new Model();
        //    Employee e = new Employee();
        //    e.FirstName = "Emil";
        //    e.LastName = "Lønneberg";
        //    e.BirthDay = new DateTime(1987, 03, 23);
        //    e.EmploymentDate = new DateTime(2018, 04, 17);
        //    e.CPRNumber = e.BirthDay.ToString("dd") + e.BirthDay.ToString("MM") + e.BirthDay.ToString("yy") + "-" + "9513";
        //    e.Salary = 13000;

        //    model.Employees.Add(e);
        //    int count = model.Employees.ToList().Count;
        //    model.SaveChanges();
        //    int newCount = model.Employees.ToList().Count;

        //    Assert.AreEqual(newCount, count + 1);
        //}

        [TestMethod]
        public void UpdateEmployee()
        {
            Model model = new Model();
            Employee e = model.Employees.Where(somename => somename.FirstName == "Emil").FirstOrDefault();
            string oldFirstName = e.FirstName;
            e.FirstName = "Jens";
            model.SaveChanges();
            var newFirstname = model.Employees.Where(somename => somename.FirstName == "Jens").FirstOrDefault();

            Assert.AreNotEqual(oldFirstName, newFirstname);
        }
    }
}

