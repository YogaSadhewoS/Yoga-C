using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Masukkan angka sebagai basis: ");
        int basis = int.Parse(Console.ReadLine());

        Console.WriteLine("Masukkan angka sebagai pangkat: ");
        int pangkat = int.Parse(Console.ReadLine());

        Console.WriteLine("Menghitung...");
        int hasilPangkat = await HitungPangkatAsync(basis, pangkat);

        Console.WriteLine($"Output: {hasilPangkat}");
        Console.WriteLine("Program selesai");
    }

    static async Task<int> HitungPangkatAsync(int a, int b)
    {
        await Task.Delay(2000);

        int hasil = 1;
        for (int i = 0; i < b; i++ )
        {
            hasil *= a;
        }

        return hasil;
    }
}