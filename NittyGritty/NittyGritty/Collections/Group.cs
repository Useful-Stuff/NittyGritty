﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NittyGritty.Collections
{
    public class Group<TKey, TItem>
    {
        public Group(TKey key) : this(key, new Collection<TItem>())
        {

        }

        public Group(TKey key, IList<TItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Key = key;
            Items = new Collection<TItem>(items);
        }

        public TKey Key { get; }

        public Collection<TItem> Items { get; }
    }
}
