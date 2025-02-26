//Day 3 Constructor

using System;

namespace ConsoleApp1.Day3
{
    class Kucing
    {
        public string Nama;
        public string Ras;
        public int Umur;

        public Kucing(string namaKucing, string rasKucing, int umurKucing)
        {
            Nama = namaKucing;
            Ras = rasKucing;
            Umur = umurKucing;
        }
        static void Main()
        {
            Kucing kucing1 = new Kucing("Larry", "Hitam", 2);
            Console.WriteLine($"Namanya {kucing1.Nama}, dia umurnya {kucing1.Umur} tahun, dan dia {kucing1.Ras}");
        }
    }
}