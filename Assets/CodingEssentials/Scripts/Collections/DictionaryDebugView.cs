using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CodingEssentials.Collections
{
    internal sealed class DictionaryDebugView<TK, TV>
    {
        private readonly IDictionary<TK, TV> _dict;

        public DictionaryDebugView(IDictionary<TK, TV> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            _dict = dictionary;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TK, TV>[] Items
        {
            get
            {
                var items = new KeyValuePair<TK, TV>[_dict.Count];
                _dict.CopyTo(items, 0);
                return items;
            }
        }
    }
}