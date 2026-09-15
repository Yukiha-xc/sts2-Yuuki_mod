using System.Collections.Generic;
using System.Collections.ObjectModel;

public class _003C_003Ez__ReadOnlyArray<T> : ReadOnlyCollection<T>
{
    public _003C_003Ez__ReadOnlyArray(T[] list) : base(list) { }
}

public class _003C_003Ez__ReadOnlySingleElementList<T> : ReadOnlyCollection<T>
{
    public _003C_003Ez__ReadOnlySingleElementList(T item) : base(new T[] { item }) { }
}
namespace global {
    public class _003C_003Ez__ReadOnlyArray<T> : ReadOnlyCollection<T>
    {
        public _003C_003Ez__ReadOnlyArray(T[] list) : base(list) { }
    }

    public class _003C_003Ez__ReadOnlySingleElementList<T> : ReadOnlyCollection<T>
    {
        public _003C_003Ez__ReadOnlySingleElementList(T item) : base(new T[] { item }) { }
    }
}
