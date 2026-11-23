using System;
using System.Collections.Generic;
using System.Text;

class Task6
{
    static bool Balanced(string expression)
    {
        var stack = new Stack<char>();
        foreach (char c in expression)
        {
            if (c == '(' || c == '[' || c == '{') stack.Push(c);
            if (c != ')' && c != ']' && c != '}') continue;
            if (stack.Count == 0) return false;
            char open = stack.Pop();
            if (open != '(' && c == ')' || open != '[' && c == ']' || open != '{' && c == '}') return false;
        }
        return stack.Count == 0;
    }

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Арифметическое выражение: ");
        string expression = Console.ReadLine();
        Console.WriteLine(Balanced(expression) ? "Скобки расставлены правильно" : "Ошибка в скобках");
    }
}
