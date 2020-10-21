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
            var path4 = "task4.txt";
            Generator.CreateNewExpr(path4);
            var a = Postfix.Translate(File.ReadAllText(path4));
            Console.WriteLine(a);
            var b = Postfix.Calculate(a);
            Console.WriteLine(b);







            return;
            for (int i = 1; i < 130; i *= 2)
            {
                var ms = 0L;
                Generator.CreateNewExpr(path4);
                for (int j = 0; j < i; j++)
                    ms += Measure4(Postfix.Translate, Postfix.Calculate, path4);
                File.AppendAllText("task4.csv", i + ";" + ms + "\n");
            }

            //Task 3
            /* 
            var path = "input.txt";
            int countLines = 1;
            int countCommands = 10;


            File.WriteAllText("measures1000.csv", "K Lines;M Commands;Total Count Commands;ms\n");
            for (int i = 1; i < 10000000; i*=2)
            {
                countLines = i;
                countCommands = 1000;
                Generator.CreateCommands(path, countLines, countCommands);
                Measure3(Executer.Execute, path, countLines, countCommands);
            }
            */
        }

        static void Measure3(Action<string> act, string path, int countLines, int countCommands)
        {
            sw.Start();
            act(path);
            sw.Stop();
            var toWrite = countLines + ";" + countCommands + ";" + countCommands * countLines + ";" + sw.ElapsedMilliseconds + "\n";
            File.AppendAllText("measures1000.csv", toWrite);
        }

        static long Measure4(Func<string, string> translate, Func<string, double> calc, string path)
        {
            var lines = File.ReadAllLines(path);
            var exprFromFile = lines[0];
            foreach (var l in lines)
            {
                if (l.Contains('='))
                {
                    var ww = l.Split();
                    exprFromFile = exprFromFile.Replace(ww[0], ww[2]);
                }
            }
            sw.Start();
            var postFixNotation = translate(exprFromFile);
            var result = calc(postFixNotation);
            sw.Stop();
            return sw.ElapsedMilliseconds;
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

        public static void CreateNewExpr(string path)
        {
            var exp = "";
            var r = 0;
            for (int i = 0; i < 3; i++)
            {
                if (rnd.Next(2) == 0)
                {
                    if (i != 0)
                        exp += operations[rnd.Next(0, 5)] + " ";
                    r = rnd.Next(6, 9);
                    exp += operations[r] + " ( " + rnd.Next(1, 100) + " ) ";
                }
                else
                {
                    r = rnd.Next(0, 5);
                    exp += (i == 0 ? rnd.Next(1, 100).ToString() + " " : "") + operations[r] + " " + (r == 5 ? rnd.Next(1, 6) : rnd.Next(1, 100)) + " ";
                }
            }
            Console.WriteLine(exp);
            File.WriteAllText(path, exp);
        }

        static string[] operations = new string[] { "+", "-", "*", "/", "^", "ln", "sqrt", "sin", "cos", };
    }

    public static class Postfix
    {
        static Dictionary<string, Func<double[], double>> op = new Dictionary<string, Func<double[], double>>()
        {
            { "+", x => x[1] + x[0] },
            { "-", x => x[1] - x[0] },
            { "*", x => x[1] * x[0] },
            { "/", x => x[1] / x[0] },
            { "^", x => Math.Pow(x[1], x[0]) },
            { "ln", x => Math.Log(x[0], Math.E) },
            { "sqrt", x => Math.Sqrt(x[0]) },
            { "sin", x => Math.Sin(x[0]) },
            { "cos", x => Math.Cos(x[0]) }
        };
        static Dictionary<string, int> dictPrior = new Dictionary<string, int>()
        {
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "^", 3 },
            { "ln", 4 },
            { "sqrt", 4 },
            { "cos", 4 },
            { "sin", 4 }
        };

        public static string Translate(string inFixExpression)
        {
            var st = new MyStack<string>();
            var q = new Queue<string>();
            string newExp = "";
            var arr = inFixExpression.Split();
            foreach (var e in arr)
            {
                if (int.TryParse(e, out int n))
                    q.Enqueue(e);
                else if (dictPrior.ContainsKey(e))
                {
                    if (st.isEmpty || st.Top() == "(")
                        st.Push(e);
                    else if (dictPrior[e] > dictPrior[st.Top()])
                        st.Push(e);
                    else
                    {
                        while (!(st.isEmpty || st.Top() == "(" || dictPrior[e] > dictPrior[st.Top()]))
                        {
                            q.Enqueue(st.Pop());
                        }
                        st.Push(e);
                    }
                }
                else if (e == "(")
                    st.Push(e);
                else if (e == ")")
                {
                    while (st.Top() != "(")
                        q.Enqueue(st.Pop());
                    st.Pop();
                }
            }
            while (!st.isEmpty)
                q.Enqueue(st.Pop());
            while (q.Count != 0)
                newExp += q.Dequeue() + (q.Count != 0 ? " " : "");
            return newExp;
        }

        public static double Calculate(string postfixNotation)
        {
            var st = new MyStack<double>();
            var split = postfixNotation.Split();
            foreach (var e in split)
            {
                if (op.ContainsKey(e))
                {
                    if (e.Length > 1)
                        st.Push(op[e](new double[] { st.Pop() }));
                    else
                        st.Push(op[e](new double[] { st.Pop(), st.Pop() }));
                }
                else st.Push(double.Parse(e));
            }
            return st.Pop();
        }
    }
}
