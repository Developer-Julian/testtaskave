using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTaskAve.Server
{
    public class LimitedQueue<T>
    {
        private int capacity;
        private Queue<T> queue;

        public LimitedQueue(int capacity)
        {
            this.capacity = capacity;
            this.queue = new Queue<T>();
        }

        public void Enqueu(T value)
        {
            this.queue.Enqueue(value);
            this.EnsureCapacity();
        }

        public List<T> GetElements()
        {
            return this.queue.ToList();
        }

        private void EnsureCapacity()
        {
            while (this.queue.Count > this.capacity)
            {
                this.queue.Dequeue();
            }
        }
    }
}
