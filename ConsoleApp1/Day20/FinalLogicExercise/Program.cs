//Final Logic Exercise

using System;
using System.Collections.Generic;
using System.Text;

class NumberGenerator
{
    private readonly Dictionary<int, string> _rules = new Dictionary<int, string>();

    public void AddRule(int divisor, string output)
    {
        if (!_rules.ContainsKey(divisor))
        {
            _rules[divisor] = output;
        }
    }

    public string Generate(int limit)
    {
        StringBuilder result = new StringBuilder();
        
        for (int n = 1; n <= limit; n++)
        {
            StringBuilder output = new StringBuilder();
            
            foreach (var rule in _rules)
            {
                if (n % rule.Key == 0)
                {
                    output.Append(rule.Value);
                }
            }
            
            result.Append(output.Length > 0 ? output.ToString() : n.ToString());
            
            if (n < limit) result.Append(", ");
        }
        
        return result.ToString();
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Masukkan jumlah angka: ");
        int x;
        while (!int.TryParse(Console.ReadLine(), out x) || x <= 0)
        {
            Console.WriteLine("Mohon masukkan angka bulat positif");
        }
        
        NumberGenerator myGenerator = new NumberGenerator();
        myGenerator.AddRule(3, "foo");
        myGenerator.AddRule(4, "baz");
        myGenerator.AddRule(5, "bar");
        myGenerator.AddRule(7, "jazz");
        myGenerator.AddRule(9, "huzz");
        
        string output = myGenerator.Generate(x);
        Console.WriteLine(output);
    }
}
