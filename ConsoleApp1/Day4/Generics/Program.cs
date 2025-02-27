//Generics

using System;

public class Kotak 
{
    public void TambahItem<T>(T item) => Console.WriteLine($"Berhasil menambahkan item: {item}");
}

public class Uang<T>
{
    public T? Data {get; set;} //Nullable sementara menghindari warning
}

class Program
{
    static void Main()
    {
        Kotak myKotak = new Kotak();
        myKotak.TambahItem(10);
        myKotak.TambahItem("Eevee");
        myKotak.TambahItem(5.50);

        Uang<int> stringUang = new Uang<int> {Data = 10000};
        Uang<string> intUang = new Uang<string> {Data = "Sepuluh ribu"};

        Console.WriteLine($"Dalam angka: {intUang.Data}");
        Console.WriteLine($"Dalam huruf: {stringUang.Data}");
    }
}