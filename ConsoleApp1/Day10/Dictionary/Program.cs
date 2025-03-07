using System;
using System.Collections.Generic;

Dictionary<string, int> barang = new Dictionary<string, int>();

barang["Beng beng"] = 2000;
barang["Kopiko"] = 500;
barang["Indomie"] = 2500;

foreach (var item in barang)
{
    Console.WriteLine($"{item.Key}: {item.Value}");
}

Console.WriteLine("\nMasukkan barang yang ingin dicari: ");
string namaBarang = Console.ReadLine() ?? "";

if (barang.ContainsKey(namaBarang))
{
    Console.WriteLine($"{namaBarang} dengan harga {barang[namaBarang]}");
}
else
{
    Console.WriteLine("Barang tidak ditemukan");
}