using System;
using System.Collections.Generic;
using System.Text;

namespace Cubse.Hub.Tests.Project.Tests
{
    [TestClass]
    public class ProjectTests : BaseTest
    {
        [TestMethod]
        public void CanLoadProjects()
        {
            var projects = this.projectService.LoadProjects((err) => { });
        }
    }
}
