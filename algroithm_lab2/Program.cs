using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace algroithm_lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var myst = new MyStack<int>();
            myst.Push(1);
            myst.Push(2);
            myst.Print();
            Console.WriteLine(myst.Pop());
            Console.WriteLine(myst.Pop());
            Console.WriteLine(myst.Pop());
        }
    }

    public class MyStack<T>
    {
        //Push(elem), Pop(), Top(), isEmpty(), Print().

        private LinkedList<T> list;
        public bool isEmpty { get; private set; }
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
            if (list.Count != 0)
            {
                var value = list.Last.Value;
                list.RemoveLast();
                return value;
            }
            else
            {
                throw new InvalidOperationException("Stack empty.");
            }
        }

        public T Top()
        {
            return list.Last.Value;
        }

        public void Print()
        {
            foreach (var e in list)
                Console.Write(e + ", ");
            Console.Write("\b\b  \n");
        }
    }
}
