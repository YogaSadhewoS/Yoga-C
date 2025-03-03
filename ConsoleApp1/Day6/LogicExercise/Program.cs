//Logic Exercise 2

using System;
using System.Text; //Untuk memudahkan penulisan string builder

Console.WriteLine("Masukkan jumlah angka: ");
int x;

while(!int.TryParse(Console.ReadLine(), out x) || x <= 0)
{
    Console.WriteLine("Mohon masukkan angka bulat positif");
}

//Terlalu banyak if else
/*
int n = 1;
while(n <= x)
{
    if(n % 7 == 0 && n % 5 == 0 && n % 3 == 0)
    {
        Console.Write("foobarjazz");
    }

    else if(n % 7 == 0 && n % 3 == 0)
    {
        Console.Write("foojazz");
    }

    else if(n % 7 == 0 && n % 5 == 0)
    {
        Console.Write("barjazz");
    }

    else if(n % 5 == 0 && n % 3 == 0)
    {
        Console.Write("foobar");
    }

    else if(n % 3 == 0)
    {
        Console.Write("foo");
    }

    else if(n % 5 == 0)
    {
        Console.Write("bar");
    }

    else if(n % 7 == 0)
    {
        Console.Write("jazz");
    }

    else
    {
        Console.Write(n);
    }

    if(n < x) Console.Write(", ");
    n++;
}*/

//Pakai string builder
for (int n = 1; n <= x; n++)
{
    StringBuilder output = new StringBuilder();
    if (n % 3 == 0) output.Append("foo");
    if (n % 5 == 0) output.Append("bar");
    if (n % 7 == 0) output.Append("jazz");

    //Jika output tidak kosong, maka print semua pengkondisian di atas yang masuk kriteria
    //Jika tidak ada yang sesuai maka print n
    Console.Write(output.Length > 0 ? output.ToString() : n.ToString());

    if (n < x) Console.Write(", ");
}

