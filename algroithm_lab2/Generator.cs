using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace algroithm_lab2
{
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
                    if (j < countCommands - 1)
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

        public static void CreateNewExpr(string path, int count)
        {
            var exp = new StringBuilder();
            var r = 0;
            for (int i = 0; i < count; i++)
            {               
                if (rnd.Next(2) == 0)
                {
                    if (i != 0)
                    {
                        exp.Append(operations[rnd.Next(0, 5)] + " ");
                        i++;
                    }
                    r = rnd.Next(6, 9);
                    exp.Append(operations[r] + " ( " + rnd.Next(1, 100) + " ) ");
                }
                else
                {
                    r = rnd.Next(0, 5);
                    exp.Append((i == 0 ? rnd.Next(1, 100).ToString() + " " : "") + operations[r] + " " + (r == 5 ? rnd.Next(1, 6) : rnd.Next(1, 100)) + " ");
                }
            }
            Console.WriteLine(exp);
            File.WriteAllText(path, exp.ToString());
        }

        static string[] operations = new string[] { "+", "-", "*", "/", "^", "ln", "sqrt", "sin", "cos", };
    }
}
