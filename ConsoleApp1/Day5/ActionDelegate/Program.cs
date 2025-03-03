//Action Delegate

using System;

class Program
{
    static void Main()
    {
        Action<string> cetakPesan = Pesan;
        cetakPesan("Selamat datang di program C#!");
        cetakPesan("Semoga ilmunya bermanfaat");

        Action<int, int> jumlah = (x, y) => Console.WriteLine($"Hasil Penjumlahan: {x + y}");
        jumlah(5, 3);
        Action<int, int> kali = (x, y) => Console.WriteLine($"Hasil Perkalian: {x*y}");
        kali(5, 3);
    }

    static void Pesan(string teks)
    {
        Console.WriteLine($"Pesan: {teks}");
    }
}