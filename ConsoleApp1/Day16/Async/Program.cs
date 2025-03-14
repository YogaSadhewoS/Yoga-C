using System;
using System.Threading.Tasks;

class Program
{
    //Basic Async Case
    /*static async Task Main()
    {
        Console.WriteLine("Masukkan angka sebagai basis: ");
        int basis = int.Parse(Console.ReadLine());

        Console.WriteLine("Masukkan angka sebagai pangkat: ");
        int pangkat = int.Parse(Console.ReadLine());

        Console.WriteLine("Menghitung...");
        int hasilPangkat = await HitungPangkatAsync(basis, pangkat);

        Console.WriteLine($"Output: {hasilPangkat}");
        Console.WriteLine("Program selesai");
    }

    static async Task<int> HitungPangkatAsync(int a, int b)
    {
        await Task.Delay(2000);

        int hasil = 1; //Agar hasil 1 ketika input 0
        for (int i = 0; i < b; i++ )
        {
            hasil *= a;
        }

        return hasil;
    }*/

    //Parallel task with Task.WhenAll()
    /*static async Task Main()
    {
        Console.WriteLine("Menyiapkan aplikasi. Mohon tunggu sebentar!");

        Task konfigurasiAplikasi = ConfigureApplication();
        Task konekKeDatabase = ConnectToDatabase();
        Task ngaturCache = CacheInitialization();

        await Task.WhenAll(konfigurasiAplikasi, konekKeDatabase, ngaturCache); //Run banyak task barengan
        Console.WriteLine("Aplikasi siap digunakan!");
    }

    static async Task ConfigureApplication()
    {
        Console.WriteLine("Memuat konfigurasi aplikasi...");
        await Task.Delay(1500);
        Console.WriteLine("Konfigurasi selesai");
    }

    static async Task ConnectToDatabase()
    {
        Console.WriteLine("Menghubungkan ke database...");
        await Task.Delay(2500);
        Console.WriteLine("Database terhubung");
    }

    static async Task CacheInitialization()
    {
        Console.WriteLine("Menginisialisasi cache...");
        await Task.Delay(1000);
        Console.WriteLine("Cache sudah diinisialisasi");
    }*/

    //Chaining Task with ContinueWith()
    static void Main()
    {
        Random rnd = new Random();

        Task<int> getRandomNumberTask = Task.Run(() =>
        {
            int angkaRandom = rnd.Next(1,101);
            Console.WriteLine($"Angka randomnya adalah: {angkaRandom}");
            return angkaRandom;
        });

        Task<int> multiplyByTwo = getRandomNumberTask.ContinueWith(task =>
        {
            int hasilKali = task.Result * 2;
            Console.WriteLine($"Hasil bilangan random dikali 2: {hasilKali}");
            return hasilKali;
        });

        Task displayTask = multiplyByTwo.ContinueWith(task =>
        {
            Console.WriteLine($"Hasil akhirnya adalah {task.Result}");
        });

        displayTask.Wait(); //Menunggu semua task selesai baru running code di bawahnya
        Console.WriteLine("Program selesai");
    }
}