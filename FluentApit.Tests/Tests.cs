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
            var results = model.Projects;



        }
    }
}
