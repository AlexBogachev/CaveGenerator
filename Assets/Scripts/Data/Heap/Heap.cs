using System;

namespace Assets.Scripts.Data
{
    public class Heap <T> where T : IHeapItem<T>
    {
        public int Count { get; private set; }
        private T[] items;

        public Heap(int maxSize)
        {
            items = new T[maxSize];
        }

        public void UpdateItem(T item)
            => SortUp(item);

        public bool Contains(T item)
            => Equals(items[item.HeapIndex], item);

        public void Add(T item)
        {
            item.HeapIndex = Count;
            items[Count] = item;
            SortUp(items[Count]);
            Count++;
        }

        private void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;
            int n = 0;
            while (true)
            {
                n++;
                if (n > 5000)
                {
                    throw new NotImplementedException("OVERFLOW");
                }
                var parent = items[parentIndex];
                if (item.CompareTo(parent) > 0)
                {
                    Swap(item, parent);
                }
                else
                {
                    break;
                }
                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        public T PopFirst()
        {
            T firstItem = items[0];
            Count--;
            items[0] = items[Count];
            items[0].HeapIndex = 0;
            SortDown(items[0]);

            return firstItem;
        }

        private void SortDown(T item)
        {
            int n = 0;
            while (true)
            {
                n++;
                if (n > 5000)
                {
                    throw new NotImplementedException("OVERFLOW");
                }
                var left = item.HeapIndex * 2 + 1;
                var right = item.HeapIndex * 2 + 2;
                var swapIndex = 0;

                if (left < Count)
                {
                    swapIndex = left;
                    if (right < Count)
                    {
                        if (items[left].CompareTo(items[right])<0)
                            swapIndex = right;
                    }

                    if (item.CompareTo(items[swapIndex])<0)
                        Swap(item, items[swapIndex]);

                    else
                        return;

                }
                else
                    return;
            }
        }

        private void Swap(T itemA, T itemB)
        {
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;

            var tmp = itemB.HeapIndex;
            itemB.HeapIndex = itemA.HeapIndex;
            itemA.HeapIndex = tmp;
        }
    }
}
