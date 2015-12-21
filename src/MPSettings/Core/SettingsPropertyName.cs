using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MPSettings.Core
{
    [DebuggerDisplay("{_Name}")]
    public struct SettingsPropertyName
    {
        private const string _RootMark = ".";

        public static readonly SettingsPropertyName Root = new SettingsPropertyName(_RootMark);

        private readonly string _Name;

        private SettingsPropertyName(string name)
        {
            _Name = name != _RootMark ? name.Trim().TrimEnd('.') : _RootMark;
        }

        public SettingsPropertyName Path
        {
            get
            {
                var lastindex = _Name.LastIndexOf('.');

                if (lastindex == -1)
                    return null;

                if (lastindex == 0)
                    return SettingsPropertyName.Root;

                return _Name.Substring(0, lastindex);
            }
        }

        public SettingsPropertyName[] PathParts
        {
            get
            {
                List<SettingsPropertyName> retval = new List<SettingsPropertyName>();
                retval.Add(Root);
                foreach (string split in _Name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    retval.Add(split);
                }

                return retval.ToArray();
            }
        }

        public static implicit operator SettingsPropertyName(string name)
        {
            return new SettingsPropertyName(name);
        }

        public override string ToString()
        {
            return _Name;
        }


        // overload operator + 
        public static SettingsPropertyName operator +(SettingsPropertyName a, SettingsPropertyName b)
        {
            if (a == Root)
            {
                return new SettingsPropertyName('.' + b._Name);
            }

            return new SettingsPropertyName(a._Name + '.' + b._Name);
        }

        public static bool operator ==(SettingsPropertyName a, SettingsPropertyName b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SettingsPropertyName a, SettingsPropertyName b)
        {
            return !a.Equals(b);
        }


        public override bool Equals(object obj)
        {
            if (!(obj is SettingsPropertyName))
                return false;

            return Equals((SettingsPropertyName)obj);
        }

        public bool Equals(SettingsPropertyName other)
        {
            return _Name.Equals(other._Name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return _Name.GetHashCode();
        }
    }
}