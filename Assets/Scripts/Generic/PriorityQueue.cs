using System;
using System.Collections.Generic;
using UnityEngine;

namespace Generic
{
    public class PriorityQueue<T>
    {
        private List<T> data;
        private IComparer<T> comparer;

        public PriorityQueue(IComparer<T> comparer)
        {
            data = new List<T>(10);
            this.comparer = comparer;
        }

        public void Enqueue(T item)
        {
            data.Add(item);
            int index = data.Count - 1;

            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;

                if (comparer.Compare(data[parentIndex], data[index]) <= 0) break;

                (data[parentIndex], data[index]) = (data[index], data[parentIndex]);
                index = parentIndex;
            }
        }
        public T Dequeue()
        {
            int lastIndex = data.Count - 1;
            T result = data[0];
            data[0] = data[lastIndex];
            data.RemoveAt(lastIndex--);

            int parentIndex = 0;

            while (true)
            {
                int leftChildIndex = parentIndex * 2 + 1;

                if (leftChildIndex > lastIndex) break;

                int rightChildIndex = leftChildIndex + 1;
                int swapIndex = leftChildIndex;

                if (rightChildIndex <= lastIndex && comparer.Compare(data[rightChildIndex], data[leftChildIndex]) < 0)
                {
                    swapIndex = rightChildIndex;
                }

                if (comparer.Compare(data[swapIndex], data[parentIndex]) > 0) break;

                (data[parentIndex], data[swapIndex]) = (data[swapIndex], data[parentIndex]);
                parentIndex = swapIndex;
            }

            return result;
        }

        public T Peek() => data[0];
        public bool IsEmpty => data.Count == 0;
        public int Count => data.Count;

        public void Clear() => data.Clear();

        public bool TryPeek(out T item)
        {
            item = IsEmpty ? default : Peek();
            return IsEmpty == false;
        }
    }
}