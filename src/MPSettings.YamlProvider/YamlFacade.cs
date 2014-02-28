using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace MPSettings.YamlProvider
{
    public class YamlFacade
    {
        private readonly string _path;

        public YamlFacade(string path)
        {
            _path = path;
        }

        private YamlStream yaml = null;

        private void LoadYaml()
        {
            using(var input = new StreamReader(_path))
            {
                yaml = new YamlStream();
                yaml.Load(input);
            }
        }

        private string GetLevel(string property, int level)
        {
            var yyy = property.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if(level >= yyy.Length)
                return null;

            return yyy[level];
        }

        public T Deserialize<T>()
        {
            YamlDotNet.Serialization.Deserializer deser = new Deserializer();

            using(var input = new StreamReader(_path))
            {
                return deser.Deserialize<T>(input);
            }
        }


        private object ObjectWalker(YamlMappingNode mappingNode, SettingsNode property, int level, ref bool found)
        {
            foreach(var node in mappingNode)
            {
                YamlScalarNode scalarnodekey = node.Key as YamlScalarNode;

                if(scalarnodekey != null && scalarnodekey.Value == property[level])
                {
                    if(level + 1 == property.Length)
                    {
                        YamlScalarNode scalarnodevalue = node.Value as YamlScalarNode;
                        
                        Debug.Assert(scalarnodevalue!=null);

                        found = true;
                        return scalarnodevalue.Value;
                    }

                    YamlMappingNode mappingnodevalue = node.Value as YamlMappingNode;

                    if(mappingnodevalue != null)
                    {
                        return ObjectWalker(mappingnodevalue, property, level + 1, ref found);
                    }

                    Debug.Assert(mappingnodevalue==null);
                
                }
            
            }

            found = false;
            return null;

        }

        public object GetObject(string property, out bool found)
        {
            if(yaml == null)
                LoadYaml();

            found = false;
            object retval = null;

            foreach(var doc in yaml.Documents)
            {
                retval = ObjectWalker((YamlMappingNode)doc.RootNode, property, 0, ref found);

                if(found)
                    break;
            }

            //if(found)
            //{ 
            //    YamlDotNet.Serialization.Utilities.TypeConverter.
            
            
            //}

            return retval;


  
        }


    }
}
