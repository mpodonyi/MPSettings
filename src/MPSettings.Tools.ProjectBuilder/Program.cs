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
            var projParent = new Project(args[0]);
            Project[] projChilds = (from i in args.Skip(1)
                                    select new Project(i)).ToArray();
            projParent.LinkTo(projChilds);
        }

        //static void Main(string[] args)
        //{
        //    var projParent = new Project(@"D:\Development\MPSettings\src\MPSettings\MPSettings.csproj");
        //    var projChild = new Project(@"D:\Development\MPSettings\src\MPSettings.Net\MPSettings.Net.csproj");
        //    projParent.LinkTo(projChild);
        //}
    }
}
