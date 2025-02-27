//Enum

using System;

public enum Hari
{
    Senin,
    Selasa,
    Rabu,
    Kamis,
    Jumat,
    Sabtu,
    Minggu
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Masukkan nama hari (Contoh: Minggu atau minggu): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Input kosong, Mohon diisi dengan benar");
        }

        else
        {
            if (Enum.TryParse(input, true, out Hari hariPilihan))
            {
                Console.WriteLine($"Hari yang anda pilih adalah hari {hariPilihan}");
            }
            else
            {
                Console.WriteLine("Terjadi kesalahan input. Mohon diulangi");
            }
        }

    }
}
