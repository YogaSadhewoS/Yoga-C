using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Task task1 = Task.Run(() =>
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
        Console.WriteLine("Program selesai");
    }
}