using System;
using System.Collections.Generic;

namespace algroithm_lab2
{
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
