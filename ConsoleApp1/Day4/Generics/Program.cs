//Generics

using System;

//Buatlah sebuah class generik Kotak<T> yang memiliki 
// method TambahItem(T item) dan AmbilItem(). 
// Method AmbilItem() akan mengembalikan item yang terakhir 
// ditambahkan. Cobalah untuk menggunakan class ini dengan 
// berbagai tipe data, misalnya int, string, dan double.
public class Kotak 
{
    public void TambahItem<T>(T item) => Console.WriteLine($"Berhasil menambahkan item: {item}");
}

class Program
{
    static void Main()
    {
        Kotak myKotak = new Kotak();
        myKotak.TambahItem(10);
        myKotak.TambahItem("Eevee");
        myKotak.TambahItem("5.50");
    }
}