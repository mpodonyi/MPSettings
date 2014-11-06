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

            var parentCompileUnits = parentProject.GetCompileUnits();
            var myCompileUnits = this.GetCompileUnits();

            var unitsConnectedWithParentProject = from i in myCompileUnits
                                                  where i.IsBasedIn(parentProject)
                                                  select i;

            var compileUnitsToRemove = unitsConnectedWithParentProject.Except(parentCompileUnits);

            var compileUnitsToAdd = parentCompileUnits.Except(unitsConnectedWithParentProject);

            foreach (var unit in compileUnitsToRemove)
            {
                RemoveCompileUnit(unit);
                dirtyflag = true;
            }

            foreach (var unit in compileUnitsToAdd)
            {
                if (!myCompileUnits.Contains<CompileUnit>(unit)) //check that no object with same name exist
                {
                    AddCompileUnit(unit);
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

        private void RemoveCompileUnit(CompileUnit compileunit)
        {
            compileunit.XmlElement.Remove();
        }


        private void AddCompileUnit(CompileUnit parentunit)
        {
            GetAddAfter().AddAfterSelf(parentunit.CreateLinkedXElementFor(this));
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
            public readonly XElement XmlElement;
            private readonly Project myproject;

            internal CompileUnit(XElement element, Project project)
            {
                myproject = project;
                XmlElement = element;
                Include = element.Attribute("Include").Value;
                var linkelement = element.Element(Constants.ns + "Link");
                if (linkelement != null)
                {
                    IsLink = true;
                    LinkPath = linkelement.Value;
                }
            }

            internal XElement CreateLinkedXElementFor(Project project)
            {
                string newInclude = Uri.UnescapeDataString(GetBaseUri(project).MakeRelativeUri(this.AbsolutePath).ToString().Replace('/', Path.DirectorySeparatorChar));
                string newLinkPath = this.IsLink ? LinkPath : Include;

                XElement retval = new XElement(Constants.ns + "Compile",
                    new XAttribute("Include", newInclude),
                    new XElement(Constants.ns + "Link", newLinkPath)
                    );

                return retval;
            }

            private Uri GetBaseUri(Project project)
            {
                string basedirectory = Path.GetDirectoryName(project.PathToProjectFile);
                if (!basedirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    basedirectory += Path.DirectorySeparatorChar;
                }

                return new Uri(basedirectory);
            }

            private Uri AbsolutePath
            {
                get
                {
                    Uri baseUri = GetBaseUri(myproject);
                    Uri fullUri = new Uri(baseUri, Include);

                    return fullUri;
                }
            }

            public bool IsBasedIn(Project project)
            {
                Uri baseUri = GetBaseUri(project);
                Uri fullUri = AbsolutePath;

                return baseUri.IsBaseOf(fullUri);
            }


            public bool Equals(CompileUnit other)
            {
                if (other == null)
                    return false;

                if (!this.IsLink && !other.IsLink)
                {
                    return this.Include.Equals(other.Include);
                }

                return this.AbsolutePath.Equals(other.AbsolutePath);
            }

            public override bool Equals(Object obj)
            {
                CompileUnit personObj = obj as CompileUnit;

                return personObj == null
                    ? false
                    : this.Equals(personObj);
            }

            public override int GetHashCode()
            {
                return this.AbsolutePath.GetHashCode();
            }
        }

        //private sealed class CompileUnitEqualityComparer : EqualityComparer<CompileUnit>
        //{
        //    public override bool Equals(CompileUnit x, CompileUnit y)
        //    {
        //        return x.Equals(y);
        //    }

        //    public override int GetHashCode(CompileUnit obj)
        //    {
        //        return obj.GetHashCode();
        //    }
        //}


    }
}
