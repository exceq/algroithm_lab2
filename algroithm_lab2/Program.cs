using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace algroithm_lab2
{
    class Program
    {
        static Stopwatch sw = new Stopwatch();
        static void Main(string[] args)
        {
            var path = "input.txt";
            int countLines = 1;
            int countCommands = 10;


            File.WriteAllText("measures1000.csv", "K Lines;M Commands;Total Count Commands;ms\n");
            for (int i = 1; i < 10000000; i*=2)
            {
                countLines = i;
                countCommands = 1000;
                Generator.CreateCommands(path, countLines, countCommands);
                Measure(Executer.Execute, path, countLines, countCommands);
            }
        }

        static void Measure(Action<string> act, string path, int countLines, int countCommands)
        {
            sw.Start();
            act(path);
            sw.Stop();
            var toWrite = countLines + ";" + countCommands + ";" + countCommands * countLines + ";" + sw.ElapsedMilliseconds + "\n";
            File.AppendAllText("measures1000.csv", toWrite);
        }
    }

    public static class Generator
    {
        static Random rnd = new Random();
        public static void CreateCommands(string path, int countLines, int countCommands)
        {
            var list = new List<string>();
            for (int i = 0; i < countLines; i++)
            {
                int countPush = 0;
                var com = "";
                for (int j = 0; j < countCommands;)
                {
                    var cName = rnd.Next() % 6;
                    switch (cName)
                    {
                        case 0:
                            continue;
                        case 1:
                            com += 1 + "," + (rnd.Next(4) > 2 ? CreateRndWord(rnd.Next(6)) : rnd.Next(1000).ToString());
                            countPush++;
                            break;
                        case 2:
                        case 3:
                            if (countPush <= 0) continue;
                            com += cName;
                            countPush--;
                            break;
                        default:
                            com += cName;
                            break;
                    }
                    if (j < countCommands-1)
                        com += " ";
                    j++;
                }
                list.Add(com);
            }
            File.WriteAllLines(path, list);
            
        }

        static string CreateRndWord(int len)
        {
            var word = "";
            for (int i = 0; i < 4; i++)
                word += (char)rnd.Next(97, 122);
            return word;
        }
    }

    public static class Executer
    {
        public static void Execute(string path)
        {
            var text = File.ReadAllLines(path);
            foreach (var line in text)
            {
                var st = new MyStack<string>();

                var a = line.Split();
                for (int i = 0; i < a.Length; i++)
                {
                    switch (a[i][0].ToString())
                    {
                        case "1":
                            st.Push(a[i].Split(',')[1]);
                            Console.Write(st.Top());
                            break;
                        case "2":
                            Console.Write(st.Pop());
                            break;
                        case "3":
                            Console.Write(st.Top());
                            break;
                        case "4":
                            Console.Write(st.isEmpty);
                            break;
                        case "5":
                            st.Print();
                            break;
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }

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
