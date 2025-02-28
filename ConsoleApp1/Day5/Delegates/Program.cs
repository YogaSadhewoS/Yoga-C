//Basic Delegates or Delegates Custom

using System;

class Program 
{   
    delegate int OperasiMatematika(int x, int y);

    static int Tambah(int a, int b) => a + b;
    static int Kurang(int a, int b) => a - b;
    static int Kali(int a, int b) => a * b;

    static void Main()
    {
        OperasiMatematika penjumlahan = Tambah;
        Console.WriteLine($"Hasil penjumlahan: {penjumlahan(10, 5)}");

        OperasiMatematika pengurangan = Kurang;
        Console.WriteLine($"Hasil pengurangan: {pengurangan(10, 5)}");

        OperasiMatematika perkalian = Kali;
        Console.WriteLine($"Hasil perkalian: {perkalian(10,5)}");
    }
}
