using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        //Basic task w/o async
        /*Task task1 = Task.Run(() =>
        {
            Console.WriteLine("Tugas 1 dimulai");
            Thread.Sleep(1000); //Pakai thread sleep karena Main() ga pake async
            Console.WriteLine("Tugas 1 selesai");
        });
        
        Task task2 = Task.Run(() =>
        {
            Console.WriteLine("Tugas 2 dimulai");
            Thread.Sleep(1500);
            Console.WriteLine("Tugas 2 selesai");
        });

        task1.Wait();
        task2.Wait();
        Console.WriteLine("Program selesai");*/

        //Generic Task<int> so the output will be int
        Console.WriteLine("Masukkan angka bulat positif: ");
        int angka;

        while(!int.TryParse(Console.ReadLine(), out angka) || angka < 0)
        {
            Console.WriteLine("Input yang bener blok");
        }

        Task<int> task = Task.Run(() =>
        {
            Console.WriteLine($"Menghitung faktorial dari angka {angka}");
            Thread.Sleep(1000);

            int result = 1;
            for (int i = angka; i > 0; i--)
            {
                result *= i;
            }
            return result;

        });

        int hasil = task.Result;
        Console.WriteLine($"Output: {hasil}");
        Console.WriteLine("Program selesai");
    }
}