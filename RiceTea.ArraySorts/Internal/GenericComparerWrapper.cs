using System.Collections;
using System.Collections.Generic;

namespace RiceTea.ArraySorts.Internal
{
    internal sealed class GenericComparerWrapper<T> : IComparer
    {
        private readonly IComparer<T> _comparer;

        public GenericComparerWrapper(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public int Compare(object x, object y)
        {
            return _comparer.Compare(x is T castedX ? castedX : default, y is T castedY ? castedY : default);
        }
    }
}
