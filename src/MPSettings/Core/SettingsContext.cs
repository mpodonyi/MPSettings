using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSettings.Core
{
    public class SettingsContext : IDictionary<object, object>
    {
        private bool _ReadonlyFlag = false;

        private static readonly Dictionary<object, object> _GlobalExternalDict = new Dictionary<object, object>();

        private readonly Dictionary<object, object> _ExternalDict;
        private readonly Dictionary<object, object> _InnerDict;

        internal SettingsContext(SettingsContext settingsContext)
        {
            _ExternalDict = settingsContext == null ? _GlobalExternalDict : settingsContext._InnerDict;

            _InnerDict = new Dictionary<object, object>();       //MP: lazy
        }

        internal SettingsContext()
            : this(null)
        {
        }

        #region IDictionary<object,object> Members

        public void Add(object key, object value)
        {
            if (_ExternalDict.ContainsKey(key))
                throw new ArgumentException();
            _InnerDict.Add(key, value);
        }

        public bool ContainsKey(object key)
        {
            if (_ExternalDict.ContainsKey(key) || _InnerDict.ContainsKey(key))
                return true;

            return false;
        }

        public ICollection<object> Keys
        {
            get
            {
                return _ExternalDict.Keys.Concat(_InnerDict.Keys).ToList();
            }
        }

        public bool Remove(object key)
        {
            throw new NotSupportedException();
        }

        public bool TryGetValue(object key, out object value)
        {
            if (_ExternalDict.TryGetValue(key, out value))
            {
                return true;
            }
            else
            {
                return _InnerDict.TryGetValue(key, out value);
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return _ExternalDict.Values.Concat(_InnerDict.Values).ToList();
            }
        }

        public object this[object key]
        {
            get
            {
                if (_ExternalDict.ContainsKey(key))
                {
                    return _ExternalDict[key];
                }

                if (_InnerDict.ContainsKey(key))
                {
                    return _InnerDict[key];
                }

                throw new KeyNotFoundException();
            }
            set
            {
                if (_ExternalDict.ContainsKey(key))
                {
                    _ExternalDict[key] = value;
                    return;
                }

                _InnerDict[key] = value;
            }

        }

        #endregion

        #region ICollection<KeyValuePair<object,object>> Members

        public void Add(KeyValuePair<object, object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(KeyValuePair<object, object> item)
        {
            if (_ExternalDict.Contains(item))
                return true;

            if (_InnerDict.Contains(item))
                return true;

            return false;
        }

        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return _ExternalDict.Count + _InnerDict.Count; }
        }

        public bool IsReadOnly
        {
            get { return _ReadonlyFlag; }
        }

        public bool Remove(KeyValuePair<object, object> item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<KeyValuePair<object,object>> Members

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            foreach (var a in _ExternalDict)
                yield return a;

            foreach (var b in _InnerDict)
                yield return b;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}