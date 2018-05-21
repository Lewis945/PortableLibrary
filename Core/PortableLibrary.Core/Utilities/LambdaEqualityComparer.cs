﻿using System;
using System.Collections.Generic;

namespace PortableLibrary.Core.Utilities
{
    public class LambdaEqualityComparer<TSource, TDest> :
        IEqualityComparer<TSource>
    {
        private readonly Func<TSource, TDest> _selector;

        public LambdaEqualityComparer(Func<TSource, TDest> selector)
        {
            _selector = selector;
        }

        public bool Equals(TSource obj, TSource other)
        {
            return _selector(obj).Equals(_selector(other));
        }

        public int GetHashCode(TSource obj)
        {
            return _selector(obj).GetHashCode();
        }
    }
}