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
        [TestMethod]
        public void InvalidOperationExceptionOnCreateUnaffiliatedContactInfo()
        {

        }
    }
}
