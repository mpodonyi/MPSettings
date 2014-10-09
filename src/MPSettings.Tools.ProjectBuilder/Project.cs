using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MPSettings.Tools.ProjectBuilder
{
    internal static class Constants
    {
        internal static readonly XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
    }

    internal class Project
    {
        private readonly string PathToProjectFile;
        private readonly XDocument Root;

        public Project(string pathToProjectFile)
        {
            if (!File.Exists(pathToProjectFile))
                throw new FileNotFoundException("Projectfile not found", pathToProjectFile);

            PathToProjectFile = pathToProjectFile;
            Root = XDocument.Load(PathToProjectFile);
        }

        private void LinkFrom(Project parentProject)
        {
            bool dirtyflag = false;

            List<CompileUnit> parentCompileUnits = parentProject.GetCompileUnits();
            List<CompileUnit> myCompileUnits = this.GetCompileUnits();

            foreach (CompileUnit parentunit in parentCompileUnits)
            {
                if (!myCompileUnits.Contains<CompileUnit>(parentunit))
                {
                    AddCompileUnit(parentunit);
                    dirtyflag = true;
                }
            }

            if (dirtyflag)
                Root.Save(PathToProjectFile);
        }


        public Project LinkTo(params Project[] projects)
        {
            foreach (var project in projects)
                project.LinkFrom(this);

            return this;
        }


        private void AddCompileUnit(CompileUnit parentunit)
        {
            GetAddAfter().AddAfterSelf(parentunit.CreateNewXElement(this));
        }

        private XElement GetAddAfter()
        {
            return Root.Element(Constants.ns + "Project").Elements(Constants.ns + "ItemGroup").Elements(Constants.ns + "Compile").Last(); //MP: create if not exist
        }

        private List<CompileUnit> GetCompileUnits()
        {
            return (from i in Root.Element(Constants.ns + "Project").Elements(Constants.ns + "ItemGroup").Elements(Constants.ns + "Compile")
                    where i.Attribute("Include").Value.EndsWith(".cs")
                    select new CompileUnit(i, this)).ToList();
        }


        private sealed class CompileUnit : IEquatable<CompileUnit>
        {
            private readonly string Include;
            private readonly string LinkPath;
            private readonly bool IsLink = false;
            private readonly XElement ThisElemeny;
            private readonly Project myproject;

            internal CompileUnit(XElement element, Project project)
            {
                myproject = project;
                ThisElemeny = element;
                Include = element.Attribute("Include").Value;
                var linkelement = element.Element(Constants.ns + "Link");
                if (linkelement != null)
                {
                    IsLink = true;
                    LinkPath = linkelement.Value;
                }
            }

            internal XElement CreateNewXElement(Project project)
            {
                string newInclude;
                string newLinkPath;

                if (this.IsLink)
                {
                    newInclude = Include;
                    newLinkPath = LinkPath;
                }
                else
                {
                    newInclude = FixPath(Include, myproject.PathToProjectFile, project.PathToProjectFile);
                    newLinkPath = Include;
                }

                XElement retval = new XElement(Constants.ns + "Compile",
                    new XAttribute("Include", newInclude),
                    new XElement(Constants.ns + "Link", newLinkPath)
                    );

                return retval;
            }

            private static string FixPath(string pathToFile, string pathToParentProject, string pathToChildProject)
            {
                string ParentDirectory = Path.GetDirectoryName(pathToParentProject);
                string ChildDirectory = Path.GetDirectoryName(pathToChildProject);

                string combinedpath = Path.Combine(ParentDirectory, pathToFile);

                Uri pathUri = new Uri(combinedpath);
                // Folders must end in a slash
                if (!ChildDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    ChildDirectory += Path.DirectorySeparatorChar;
                }
                Uri folderUri = new Uri(ChildDirectory);
                return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
            }

            private string AbsolutePath
            {
                get 
                {
                    string basedirectory = Path.GetDirectoryName(myproject.PathToProjectFile);
                    if (!basedirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        basedirectory += Path.DirectorySeparatorChar;
                    }

                    Uri baseUri = new Uri(basedirectory);
                    Uri fullUri = new Uri(baseUri, Include);

                    return fullUri.ToString();
                }
            }


            public bool Equals(CompileUnit other)
            {
                if (other == null)
                    return false;

                if (this.IsLink && other.IsLink)
                {
                    return this.AbsolutePath.Equals(other.AbsolutePath);
                }
                else if (this.IsLink && !other.IsLink)
                {
                    return this.AbsolutePath.Equals(other.AbsolutePath);
                }
                else if (!this.IsLink && other.IsLink)
                {
                    return this.AbsolutePath.Equals(other.AbsolutePath);
                }
                else if (!this.IsLink && !other.IsLink)
                {
                    return this.Include.Equals(other.Include);
                }

                throw new NotSupportedException();


                //string nameA = this.IsLink ? this.LinkPath : this.Include;
                //string nameB = other.IsLink ? other.LinkPath : other.Include;

                //return nameA.Equals(nameB);



                
                //return this.AbsolutePath.Equals(other.AbsolutePath);
            }

            public override int GetHashCode()
            {
                return this.AbsolutePath.GetHashCode();
            }


        }

        private sealed class CompileUnitEqualityComparer : EqualityComparer<CompileUnit>
        {
            public override bool Equals(CompileUnit x, CompileUnit y)
            {
                return x.Equals(y);
            }

            public override int GetHashCode(CompileUnit obj)
            {
                return obj.GetHashCode();
            }
        }


    }
}
