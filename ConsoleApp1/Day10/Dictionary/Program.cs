using System;
using System.Collections.Generic;

//Case 1
/*Dictionary<string, int> barang = new Dictionary<string, int>();

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
}*/

//Case 2
Dictionary<string, string> mataKuliah = new Dictionary<string, string>();

mataKuliah["FIS"] = "Fisika";
mataKuliah["KIM"] = "Kimia";
mataKuliah["BIO"] = "Biologi";
mataKuliah["MAT"] = "Matematika";
mataKuliah["OLR"] = "Olahraga";
mataKuliah.Add("PROG", "Pemrograman Jaringan");

foreach (var matkul in mataKuliah)
{
    Console.WriteLine($"Matkul {matkul.Value} dengan kode {matkul.Key}");
}

Console.Write("Masukkan kode matkul: ");
string matkul = Console.ReadLine() ?? "";

if (mataKuliah.ContainsKey(matkul))
{
    Console.WriteLine($"Kode {matkul} adalah matkul {mataKuliah[matkul]}");
}
else
{
    Console.WriteLine("Kode matkul tidak ditemukan.");
    Console.Write("Apakah ingin menambahkan mata kuliah baru? (y/n): ");
    string pilihan = Console.ReadLine()?.ToLower() ?? "";
    
    if (pilihan == "y") //User menambah data baru tapi sementara
    {
        Console.Write("Masukkan nama mata kuliah baru: ");
        string namaMatkul = Console.ReadLine() ?? "";

        mataKuliah[matkul] = namaMatkul;
        Console.WriteLine($"Mata kuliah dengan kode {matkul} telah ditambahkan: {namaMatkul}");
    }
    else
    {
        Console.WriteLine("Tidak ada perubahan pada daftar mata kuliah.");
    }
}