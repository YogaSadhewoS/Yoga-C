//Basic Delegates or Delegates Custom

using System;

class Program 
{   
    //Tipe data int
    /*delegate int OperasiMatematika(int x, int y);

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
    }*/
    
    //Tipe data string
    /*delegate void TampilkanInfo(string data);

    static void TampilkanNama(string nama)
    {
        Console.WriteLine($"Nama: {nama}");
    }

    static void TampilkanUsia(string usia)
    {
        Console.WriteLine($"Usia: {usia} tahun");
    }

    static void Main()
    {
        TampilkanInfo infoNama = TampilkanNama;
        TampilkanInfo infoUsia = TampilkanUsia;

        infoNama("Agoy");
        infoUsia("23");
    }*/

    delegate bool ValidasiData(string input);

    static void Main()
    {
        Console.Write("Masukkan username: ");
        string input = Console.ReadLine();

        Console.Write("Masukkan password: ");
        string input2 = Console.ReadLine();

        ValidasiData validasiUsername = CekUsername;
        ValidasiData validasiPassword = CekPassword;

        Console.WriteLine($"validasi username: {(validasiUsername(input) ? "Valid" : "Tidak Valid")}");
        Console.WriteLine($"validasi password: {(validasiPassword(input2) ? "Valid" : "Tidak Valid")}");
    }

    static bool CekUsername(string input)
    {
        // Validasi Username: Hanya huruf & minimal 4 karakter
        return input.Length > 4 && input.All(char.IsLetter);
    }

    static bool CekPassword(string input)
    {
        // Validasi Password: Minimal 6 karakter & harus mengandung angka
        return input.Length > 6 && input.Any(char.IsDigit);
    }
}
