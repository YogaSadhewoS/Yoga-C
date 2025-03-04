using System;

//Menampilkan Tanggal dan Waktu saat ini
DateTime now = DateTime.Now;

Console.WriteLine($"Sekarang tanggal {DateTime.Now}"); //Output 3/4/2025 11:02:23 AM
Console.WriteLine($"Sekarang hari {now.ToString("dddd, dd MMMM yyyy HH:mm:ss", new System.Globalization.CultureInfo("id-ID"))}"); //Output Selasa, 04 Maret 2025 11.02.23

//Menghitung selisih hari
DateTime tgl1 = new DateTime(2025, 3, 1);
DateTime tgl2 = new DateTime(2025, 3, 4);

TimeSpan selisih = tgl2 -tgl1;
Console.WriteLine($"Selisih: {selisih.Days} hari");

//Mengecek tahun kabisat
Console.WriteLine("Masukkan tahun: ");
int kabisat;

while (!int.TryParse(Console.ReadLine(), out kabisat))
{
    Console.WriteLine("Mohon masukkan angka");
}

if (DateTime.IsLeapYear(kabisat) == true)
{
    Console.WriteLine($"{kabisat} adalah tahun kabisat");
}
else
{
    Console.WriteLine($"{kabisat} bukan tahun kabisat");
}

