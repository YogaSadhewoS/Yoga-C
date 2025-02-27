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
        Hari hariIni = Hari.Kamis;
        Console.WriteLine($"Hari ini adalah hari {hariIni}");
    }
}
