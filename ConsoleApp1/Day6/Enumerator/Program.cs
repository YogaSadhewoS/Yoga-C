using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        //Menggunakan foreach
        List<string> names = new List<string> { "Alice", "Bob", "Charlie", "David" };

        foreach (string name in names)
        {
            Console.Write(name + ", ");
        }
    }
}