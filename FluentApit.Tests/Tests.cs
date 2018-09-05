using System;
using FluentApi.EF;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentApit.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void GetAllProjects()
        {
            Model model = new Model();
            var results = model.Projects.ToList();

            Assert.AreNotEqual(results.Count, 0);

        }
        [TestMethod]
        public void AddNewProject()
        {
            Model model = new Model();
            Project p = new Project();
            p.Name = "Jens";
            p.Description = "Jens's Store";
            model.Projects.Add(p);
            int count = model.Projects.ToList().Count;
            model.SaveChanges();
            int newcount = model.Projects.ToList().Count;

            Assert.AreEqual(newcount, count + 1);
        }

        [TestMethod]
        public void UpdateProject()
        {
            Model model = new Model();
            Project p = model.Projects.Find(1);
            string oldDescription = p.Description;
            p.Description = ((new Random()).Next(0, Int32.MaxValue)).ToString();
            model.SaveChanges();
            var newP = model.Projects.Find(1);

            Assert.AreNotEqual(oldDescription, newP);
        }
        [TestMethod]
        public void CreateUnaffiliatedTeam()
        {
            Team t = new Team();
            t.Name = "A team";
            Model model = new Model();
            model.Teams.Add(t);
            model.SaveChanges();

        }

        [TestMethod]
        public void CreateAffiliatedTeam()
        {
            Team t = new Team();
            t.Name = "b Team";
            Project p = new Project();
            p.Name = "B Team Project";
            List<Team> demTeams = new List<Team>();
            demTeams.Add(t);
            p.Teams = demTeams;
            Model model = new Model();
            model.Projects.Add(p);
            model.SaveChanges();

            Team team = model.Teams.Where(someteam => someteam.Name == "b Team").FirstOrDefault();
            Project affiliatedProject = team.Project;

            bool projectHasTeam = team.Name == t.Name && affiliatedProject.Name == p.Name;
            Assert.IsTrue(projectHasTeam);
        }
        [ExpectedException(typeof(Exception),AllowDerivedTypes = true)]
        [TestMethod]
        public void InvalidOperationExceptionOnCreateUnaffiliatedContactInfo()
        {
            ContactInfo contactInfo = new ContactInfo();
            contactInfo.Email = "Mara@aspit.dk";
            contactInfo.Phone = "12345678";
            Model model = new Model();
            model.ContactInfos.Add(contactInfo);
            model.SaveChanges();
        }

        [TestMethod]
        public void CreateEmployeeWithoutContactInfo()
        {
            Model model = new Model();
            string newName = "martin";
            Employee newEmployee = new Employee { Name = newName };
            model.Employees.Add(newEmployee);
            
            
            model.SaveChanges();
            Employee existingEmployee = model.Employees.Single(e => e.Name == newName);

            Assert.AreEqual(newEmployee.Name, existingEmployee.Name);

        }
        [TestMethod]
        public void CreateEmployeeWithContactInfo()
        {
            Model model = new Model();
            string newName = "test2name";
            Employee newEmployee = new Employee { Name = newName };
            ContactInfo newContactInfo = new ContactInfo { Email = "test2mail@aspit.dk", Phone = "12344321" };
            newEmployee.ContactInfo = newContactInfo;
            model.Employees.Add(newEmployee);

            model.SaveChanges();
            Employee existingEmployee = model.Employees.Single(e => e.Name == newName);
            ContactInfo existingContactInfo = existingEmployee.ContactInfo;

            Assert.AreEqual(newEmployee.Name, existingEmployee.Name);
            Assert.AreEqual(newContactInfo.Phone, existingContactInfo.Phone);
        }
    }
}
