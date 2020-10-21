using System;
using System.Collections.Generic;

namespace algroithm_lab2
{
    public class MyStack<T>
    {
        private LinkedList<T> list;
        public bool isEmpty { get { return list.Count == 0; } }
        public MyStack()
        {
            list = new LinkedList<T>();
        }
        public void Push(T elem)
        {
            list.AddLast(elem);
        }

        public T Pop()
        {
            if (isEmpty)
                throw new InvalidOperationException("Stack empty.");

            var value = list.Last.Value;
            list.RemoveLast();
            return value;
        }

        public T Top()
        {
            if (isEmpty)
                throw new InvalidOperationException("Stack empty.");

            return list.Last.Value;
        }

        public void Print()
        {
            if (isEmpty)
                Console.Write("[]");
            else
            {
                Console.Write("[");
                foreach (var e in list)
                    Console.Write(e + ", ");
                Console.Write("\b\b]");
            }
        }
    }
}
