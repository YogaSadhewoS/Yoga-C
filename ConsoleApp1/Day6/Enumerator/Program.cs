using System;
using System.Collections;
using System.Collections.Generic;

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
        /*int[] numbers = { 10, 20, 30, 40, 50 };
        IEnumerator enumerator = numbers.GetEnumerator(); //GetEnumerator() mengembalikan non-generic IEnumerator

        while (enumerator.MoveNext())
        {
            int num = (int)enumerator.Current; //Sehingga harus casting dari object ke int
            Console.Write(num + ", ");
        }*/

        //Menggunakan yield return
        List<int> numbers = new List<int> { 12, 7, 9, 21, 30, 42, 55, 67, 88, 91 };

        foreach (int num in GetOddNumbers(numbers))
        {
            Console.Write(num + ", ");
        }

    }

    static IEnumerable<int> GetOddNumbers(IEnumerable<int> numbers)
    {
        foreach (int num in numbers)
        {
            if (num % 2 == 1)
            {
                yield return num; //Mengembalikan num satu per satu
            }
        }
    }
}