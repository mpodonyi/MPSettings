using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YamlSettingsProvider.Test
{
    class xyz
    {
        public xyz()
        { }

        public bool foo
        {
            get;
            set;
        }
    }


    [TestClass]
    public class UnitTest1
    {
        private string MyPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.yaml");
            }
        }




        [TestMethod]
        public void YamlTest()
        {

            var yaml = new YamlFacade(MyPath);

            xyz o = yaml.Deserialize<xyz>();


            //bool found;
            //object value = yaml.GetObject("foo", out found);

        }
    }
}
