//Inheritance
using System;

public class Hewan {
    public required string Nama {get; set;}
    public int Umur {get; set;}
}

public class Kucing : Hewan {
    public required string Ras {get; set;}
    public void Miaw()
    {
        Console.WriteLine("Mew mew");
    }
}

class Program 
{
    static void Main()
    {
        Kucing myKucing = new Kucing {Nama = "Ethel", Umur = 3, Ras = "Mujaer"};
        Console.WriteLine($"{myKucing.Nama} merupakan kucing ras {myKucing.Ras} yang berumur {myKucing.Umur} tahun");
    }
}
