//Action Delegate

using System;

class Program
{
    static void Main()
    {
        Action<string> cetakPesan = Pesan;
        cetakPesan("Selamat datang di program C#!");
        cetakPesan("Semoga ilmunya bermanfaat");
    }

    static void Pesan(string teks)
    {
        Console.WriteLine($"Pesan: {teks}");
    }
}