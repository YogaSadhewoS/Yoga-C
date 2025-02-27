//Inheritance
using System;

public class Hewan {
    public string Nama {get; set;}
    public int Umur {get; set;}

    public Hewan(string nama, int umur)
    {
        Nama = nama;
        Umur = umur;
    }
    public virtual void suaraHewan() //Polymorphism virtual
    {
        Console.WriteLine("Ini suara");
    }
}

public class Kucing : Hewan {
    public string Ras {get; set;}
    public Kucing(string nama, int umur, string ras) : base(nama, umur) //Constructor in Inheritance
    {
        Ras = ras;
    }
    public override void suaraHewan() //Polymorphism override
    {
        Console.WriteLine("Mew mew");
    }
}

class Program 
{
    static void Main()
    {
        Kucing myKucing = new Kucing ("Ethel", 3, "Mujaer");
        Console.WriteLine($"{myKucing.Nama} merupakan kucing ras {myKucing.Ras} yang berumur {myKucing.Umur} tahun");
        myKucing.suaraHewan();
    }
}
