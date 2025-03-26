using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {
        string formula = File.ReadAllText("formula.txt");
        Stack<string> formulaStack = new Stack<string>();
        Stack<int> numberStack = new Stack<int>();
        formulaStack.Push(formula);
        while (formulaStack.Count > 0)
        {
            string current = formulaStack.Pop();
            current = new string(current.Where(c => !char.IsWhiteSpace(c)).ToArray());
            if (current.Length == 1 && current.All(char.IsDigit))
            {
                numberStack.Push(int.Parse(current));
            }
            else if (current.StartsWith("p(") || current.StartsWith("m("))
            {
                char op = current[0];
                int zapyatayaPos = current.Skip(2)
                    .Select((c, i) => new { Char = c, Index = i })
                    .FirstOrDefault(x => x.Char == ',' &&
                        current.Substring(2, x.Index).Count(ch => ch == '(') ==
                        current.Substring(2, x.Index).Count(ch => ch == ')')).Index + 2;
                string left = current.Substring(2, zapyatayaPos - 2);
                string right = current.Substring(zapyatayaPos + 1, current.Length - zapyatayaPos - 2);
                formulaStack.Push(op.ToString());
                formulaStack.Push(left);
                formulaStack.Push(right);
            }
            else if (current == "p")
            {
                if (numberStack.Count >= 2)
                {
                    int b = numberStack.Pop();
                    int a = numberStack.Pop();
                    numberStack.Push((a + b) % 10);
                }
            }
            else if (current == "m")
            {
                if (numberStack.Count >= 2)
                {
                    int b = numberStack.Pop();
                    int a = numberStack.Pop();
                    numberStack.Push((a - b + 10) % 10);
                }
            }
        }
        if (numberStack.Count == 1)
        {
            Console.WriteLine(numberStack.Pop());
        }
    }
}