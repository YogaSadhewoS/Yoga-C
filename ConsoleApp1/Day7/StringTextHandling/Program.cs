using System;

Console.WriteLine("Masukkan kata: ");
string? input = Console.ReadLine() ?? ""; //Jika null maka string kosong

//Membalik string
char[]? ca = input.ToCharArray();
Array.Reverse(ca);

string s = new string(ca);
Console.WriteLine($"Kata yang dibalik: {s}");

//Mengecek palindrom
if (input == s)
{
    Console.WriteLine($"Kata \"{input}\" adalah palindrom");
}

else
{
    Console.WriteLine($"Kata \"{input}\" bukan palindrom");
}