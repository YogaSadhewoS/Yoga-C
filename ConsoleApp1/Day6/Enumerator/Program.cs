using System;
using System.Collections;

class Program
{
    static void Main()
    {
        //Menggunakan foreach
        /*List<string> names = new List<string> { "Alice", "Bob", "Charlie", "David" };

        foreach (string name in names)
        {
            Console.Write(name + ", ");
        }*/

        //Menggunakan enumerator manual
        int[] numbers = { 10, 20, 30, 40, 50 };
        IEnumerator enumerator = numbers.GetEnumerator();

        while (enumerator.MoveNext())
        {
            int num = (int)enumerator.Current;
            Console.Write(num + ", ");
        }

    }
}