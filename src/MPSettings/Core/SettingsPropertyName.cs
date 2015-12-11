using System;
using System.Diagnostics;

namespace MPSettings.Core
{
    [DebuggerDisplay("{_Name}")]
    public struct SettingsPropertyName
    {
        private readonly string _Name;

        public SettingsPropertyName(string name)
        {
            _Name = name.Trim().Trim('.');
        }
        
        public string Name
        {
            get
            {
                var lastindex = _Name.LastIndexOf('.');

                return lastindex == -1
                    ? _Name
                    : _Name.Substring(lastindex+1);
            }
        }

        public string Path
        {
            get
            {
                var lastindex = _Name.LastIndexOf('.');

                return lastindex == -1
                    ? null
                    : _Name.Substring(0,lastindex);
            }
        }




        public string[] PathParts
        {
            get
            {
                return _Name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }


        public static implicit operator SettingsPropertyName(string name)
        {
            return new SettingsPropertyName(name);
        }

        // overload operator + 
        public static SettingsPropertyName operator +(SettingsPropertyName a, SettingsPropertyName b)
        {
            return new SettingsPropertyName(a._Name + '.' + b._Name);
        }

    }
}