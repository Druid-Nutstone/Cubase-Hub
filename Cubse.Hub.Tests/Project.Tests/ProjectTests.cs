using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        [TestMethod]
        public void Test_Process()
        {
            var t = Process.GetProcesses().Select(x => x.ProcessName).ToList();



            var process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.Equals("Cubase15.exe", StringComparison.OrdinalIgnoreCase));


        }
    }
}
