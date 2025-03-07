using System;

List<string> names = new List<string> {"Yoga", "Ogay", "Gayo"}; //Case sensitive
names.Add("Ayog");
names.Remove("Ogay");

foreach (var name in names)
{
    Console.Write($"{name}, ");
}

Console.Write("Masukkan nama yang ingin dicari: ");
string searchName = Console.ReadLine() ?? ""; //Menghindari warning

if (names.Contains(searchName)) //Cek data di list
{
    int index = names.IndexOf(searchName); //Nyari posisi data di list
    Console.WriteLine($"Nama {searchName} ditemukan di indeks {index}");
}
else
{
    Console.WriteLine($"Nama {searchName} tidak ditemukan");
}
