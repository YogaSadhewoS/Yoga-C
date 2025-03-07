using System;

//List<string> Case
/*List<string> names = new List<string> {"Yoga", "Ogay", "Gayo"}; //Case sensitive
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
}*/

//List<int> Case
List<int> numbers = new List<int>();

for (int i = 0; i < 6; i++) //Biar user bisa masukin banyak input
{
    Console.Write($"Masukkan angka ke-{i + 1}: ");
    if (int.TryParse(Console.ReadLine(), out int input))
    {
        numbers.Add(input);
    }
    else
    {
        Console.WriteLine("Input tidak valid, mohon masukkan angka");
        i--; //Mengulangi input karena tidak valid
    }
}
double rataRata = numbers.Average();
Console.WriteLine($"\nRata-rata dari bilangan yang dimasukkan: {rataRata:F2}"); //2 angka belakang koma
