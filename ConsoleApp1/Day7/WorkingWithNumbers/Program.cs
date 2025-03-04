using System;

//Operasi Math Class
Console.WriteLine("Masukkan angka: ");
double x;

while (!double.TryParse(Console.ReadLine(), out x))
{
    Console.WriteLine("Mohon masukkan angka");
}

Console.WriteLine($"Akar kuadrat: {Math.Sqrt(x)}");
Console.WriteLine($"Nilai absolut: {Math.Abs(x)}");
Console.WriteLine($"Dibulatkan ke atas: {Math.Ceiling(x)}");
Console.WriteLine($"Dibulatkan ke bawah: {Math.Floor(x)}");

//Generate angka random
Random rnd = new Random();

Console.Write("Lima angka acak: ");
for(int i=0; i<5; i++)
{
    int angka = rnd.Next(1, 101); //Angka random 1-100
    Console.Write(angka + (i < 4 ? ", " : "\n"));
}