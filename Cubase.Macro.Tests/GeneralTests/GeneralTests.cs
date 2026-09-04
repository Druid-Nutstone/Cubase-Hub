using System.Diagnostics;

namespace Cubase.Macro.Tests.GeneralTests
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        public void Test_Processes()
        {
            var windows = Process.GetProcesses().Select(x => x.MainWindowTitle);
        }

    }
}
