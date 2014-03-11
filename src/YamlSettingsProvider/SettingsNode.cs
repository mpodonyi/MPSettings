using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YamlSettingsProvider
{

    public struct SettingsNode
    {
        private readonly string _node;

        public SettingsNode(string node)
        {
            _node = node;
            splittednode = null;
        }

        private string[] splittednode;
        private void SplitNode()
        {
            splittednode = _node.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

        }

        public int Length
        {
            get
            {
                if(splittednode == null)
                    SplitNode();
                return splittednode.Length;
            }
        }

        public string this[int index]
        {
            get
            {
                if(splittednode == null)
                    SplitNode();

                if(index >= splittednode.Length)
                    return null;

                return splittednode[index];
            }
        }

        public static implicit operator SettingsNode(string s)
        {
            return new SettingsNode(s);
        }



    }


}
