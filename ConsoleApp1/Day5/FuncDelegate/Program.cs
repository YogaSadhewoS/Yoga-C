//Delegate best practice & simple way - Func & Action

using System;

class Program
{
    /*
    static void Main()
    {
        //Func operasiMatematika ditulis sekali
        Func<int, int, int> operasiMatematika = (x, y) => x + y; //Langsung pakai ekspresi Lambda (=>)
        Console.WriteLine($"Hasil Penjumlahan: {operasiMatematika(6, 4)}");

        //Func operasiMatematika di-assign ke metode lain
        operasiMatematika = Perkalian; //Assign ke metode perkalian
        Console.WriteLine($"Hasil Perkalian: {operasiMatematika(6, 4)}");
    }

    static int Perkalian(int x, int y)
    {
        return x * y;
    }*/

    static void Main()
    {
        Console.Write("Masukkan angka: ");
        int input = Convert.ToInt32(Console.ReadLine());

        Func<int, bool> validasiAngka;
        validasiAngka = Genap;
        Console.WriteLine($"Bilangan {(validasiAngka(input) ? "Genap" : "Ganjil")}"); 
    }

    static bool Genap(int x)
    {
        return x % 2 == 0;
    }
}