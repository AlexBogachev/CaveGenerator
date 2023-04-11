using System;

namespace Assets.Scripts.Data
{
    public interface IHeapItem <T> : IComparable <T>
    {
        int HeapIndex { get; set; }
    }
}
