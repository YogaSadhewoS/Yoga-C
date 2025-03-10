//Logic Exercise 3

using System;
using System.Text; //Untuk memudahkan penulisan string builder

Console.WriteLine("Masukkan jumlah angka: ");
int x;

while(!int.TryParse(Console.ReadLine(), out x) || x <= 0)
{
    Console.WriteLine("Mohon masukkan angka bulat positif");
}

//Pakai string builder
for (int n = 1; n <= x; n++)
{
    StringBuilder output = new StringBuilder();
    if (n % 3 == 0) output.Append("foo");
    if (n % 4 == 0) output.Append("baz");
    if (n % 5 == 0) output.Append("bar");
    if (n % 7 == 0) output.Append("jazz");
    if (n % 9 == 0) output.Append("huzz");

    //Jika output tidak kosong, maka print semua pengkondisian di atas yang masuk kriteria
    //Jika tidak ada yang sesuai maka print n
    Console.Write(output.Length > 0 ? output.ToString() : n.ToString());

    if (n < x) Console.Write(", ");
}

