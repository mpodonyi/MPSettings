using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSettings.Tools.ProjectBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var projParent = new Project(@"D:\Development\MPSettings\src\MPSettings\MPSettings.csproj");
            var projChild = new Project(@"D:\Development\MPSettings\src\MPSettings.Net\MPSettings.Net.csproj");
            //projParent.LinkTo(projChild);
            string result  = Project.CompileUnit.FixPath(@"Properties\AssemblyInfo.cs",@"D:\Development\MPSettings\src\MPSettings\MPSettings.csproj",@"D:\Development\MPSettings\src\MPSettings.Net\MPSettings.Net.csproj");

         
        }
    }
}
