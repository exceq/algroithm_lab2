using System;
using System.IO;

namespace algroithm_lab2
{
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
}
