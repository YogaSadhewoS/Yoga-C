//Delegate best practice & simple way - Func & Action

using System;

class Program
{
    static void Main()
    {
        Func<int, int, int> operasiPenjumlahan = (x, y) => x + y; //Langsung pakai ekspresi Lambda (=>)
        Console.WriteLine($"Hasil Penjumlahan: {operasiPenjumlahan(6, 4)}");

        Func<int, int, int> operasiPerkalian = Perkalian; //Passing ke metode perkalian
        Console.WriteLine($"Hasil Perkalian: {operasiPerkalian(6, 4)}");
    }

    static int Perkalian(int x, int y)
    {
        return x * y;
    }
}