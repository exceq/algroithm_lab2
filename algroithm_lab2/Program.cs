using System;
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
}
