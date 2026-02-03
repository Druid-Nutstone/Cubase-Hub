using System;
using System.Collections.Generic;
using System.Text;

namespace Cubse.Hub.Tests.Config.Tests
{
    [TestClass]
    public class ConfigTests : BaseTest
    {

        [TestMethod]
        public void CanLoadConfig()
        {
            var config = this.configurationService.LoadConfiguration(() => { });
        }
    }
}
